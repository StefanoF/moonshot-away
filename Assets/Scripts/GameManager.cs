using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public PlayerState playerState;
    public StructureState helperStructureState;
    public StructureState platformStructureState;
    public StructureState rocketStructureState;

    [Header("Universe Generation")]
    public float sphereRadius;
    public List<Transform> itemPositions;
    private List<Transform> alreadyPositioned;
    private List<GameObject> items;
    private List<GameObject> lastItems;
    private Transform parentOfItems;
    private bool firstGeneration;
    private Coroutine universeCo;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        playerState.Reset();
        helperStructureState.Reset();
        platformStructureState.Reset();
        rocketStructureState.Reset();

        alreadyPositioned = new List<Transform>();
        lastItems = new List<GameObject>();
        items = new List<GameObject>();
        foreach (Transform child in transform) {
            if (child.gameObject.name == "Items") {
                parentOfItems = child;
                foreach (Transform childOfItems in child) {
                    childOfItems.gameObject.SetActive(false);
                    items.Add(childOfItems.gameObject);
                }
            }
        }
        firstGeneration = true;
        ChangeUniverseInternal();
    }

    public void ChangeUniverse() {
        if (universeCo != null) {
            StopCoroutine(universeCo);
        }
        universeCo = StartCoroutine(ChangeUniverseDelayed());
    }

    IEnumerator ChangeUniverseDelayed() {
        yield return new WaitForSeconds(1f);
        ChangeUniverseInternal();
    }

    private void ChangeUniverseInternal() {
        if (items.Count == 0) {
            print("GM ChangeUniverse need items");
            return;
        }

        foreach(GameObject lastItem in lastItems) {
            if (!playerState.inventory.Contains(lastItem) && 
            !helperStructureState.inventory.Contains(lastItem) && 
            !platformStructureState.inventory.Contains(lastItem) && 
            !rocketStructureState.inventory.Contains(lastItem) &&
            !items.Contains(lastItem))
            {
                Destroy(lastItem);
            }

            if (items.Contains(lastItem)) {
                lastItem.SetActive(false);
            }
        }

        lastItems.Clear();
        alreadyPositioned.Clear();
        items = items.OrderBy( x => Random.value ).ToList();
        itemPositions = itemPositions.OrderBy( x => Random.value ).ToList();

        foreach(GameObject item in items) {
            GameObject obj = item;
            if (!firstGeneration) {
                obj = Instantiate(item);
            }
            lastItems.Add(obj);
            obj.transform.parent = parentOfItems;
            obj.SetActive(false);

            Interact interactScript = obj.GetComponent<Interact>();
            if (interactScript.interactName == "gate_complex" || 
            interactScript.interactName == "satelliteDish_detailed" || 
            interactScript.interactName == "machine_barrel" || 
            interactScript.interactName == "machine_generator" || 
            interactScript.interactName == "barrels") 
            {
                RelocateObj(obj);
            }
            else {
                if (alreadyPositioned.Count < itemPositions.Count - 5) {
                    RelocateObj(obj);
                }
            }
        }
        firstGeneration = false;
    }

    private void RelocateObj(GameObject obj) {
        Transform randomPos = GetNextPos();
        obj.transform.position = randomPos.position;
        obj.SetActive(true);
    }

    private Transform GetNextPos() {
        Transform randomPos = itemPositions[alreadyPositioned.Count];
        alreadyPositioned.Add(randomPos);
        return randomPos;
    }

    public void ReloadScene() {
        SceneManager.LoadScene(0);
    }
}
