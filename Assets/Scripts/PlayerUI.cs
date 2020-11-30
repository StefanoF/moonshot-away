using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public List<GameObject> images;
    public PlayerState playerState;

    private void Awake() {
        images = new List<GameObject>();
        foreach (Transform child in transform) {
            images.Add(child.gameObject);
        }
    }

    public void UpdateInventoryUI() {
        print("PlayerUI: UpdateInventoryUI");
        if (playerState.checkHasItems() || playerState.checkFull()) {
            foreach(GameObject image in images) {
                bool notFoundItem = true;
                foreach(GameObject item in playerState.inventory) {
                    if (item.GetComponent<Interact>().interactName == image.name.Substring("Image_".Length)) {
                        image.SetActive(true);
                        int counter = playerState.countItemsByName(item.GetComponent<Interact>().interactName);
                        if (counter > 1) {
                            image.GetComponentInChildren<Text>().text = counter.ToString();
                        }
                        else {
                            image.GetComponentInChildren<Text>().text = "";
                        }
                        notFoundItem = false;
                    }
                }
                if (notFoundItem) {
                    image.SetActive(false);
                }
            }
            
        }
        

        if (playerState.checkEmpty()) {
            foreach(GameObject image in images) {
                image.SetActive(false);
            }
        }
    }
}
