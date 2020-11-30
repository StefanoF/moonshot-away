using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StructureUI : MonoBehaviour
{
    public List<GameObject> images;
    public StructureState structureState;
    public StructureCreator structureCreator;
    public Text inventoryCounterText;
    public Text resultCounterText;
    public GameObject craftButton;
    public GameObject confirmButton;
    public Image platformResult;
    public Text invalidCombo;
    public AudioSource alertSound;
    public AudioSource dingSound;
    private Coroutine flashingAlert;

    private void Awake() {
        images = new List<GameObject>();
        foreach (Transform child in transform) {
            images.Add(child.gameObject);
        }
    }

    public void UpdateInventoryUI() {
        if (structureState.checkHasItems() || structureState.checkFull()) {
            foreach(GameObject image in images) {
                bool notFoundItem = true;
                foreach(GameObject item in structureState.inventory) {
                    if (item.GetComponent<Interact>().interactName == image.name.Substring("Image_".Length)) {
                        image.SetActive(true);
                        int counter = structureState.countItemsByName(item.GetComponent<Interact>().interactName);
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
        

        if (structureState.checkEmpty()) {
            foreach(GameObject image in images) {
                image.SetActive(false);
            }
        }

        if (structureState.checkFull()) {
            craftButton.SetActive(true);
        }
        else {
            craftButton.SetActive(false);
            confirmButton.SetActive(false);
        }

        inventoryCounterText.text = structureState.inventory.Count.ToString() + "/" + structureState.capacity;
    }

    public void UpdateInventoryResult() {
        GameObject comboResult = structureCreator.VerifyCombination();
        if (comboResult) {
            print("StructureUI comboResult: " + comboResult.name);
            foreach(GameObject image in images) {
                if (comboResult.name.Contains(image.name.Substring("Image_".Length))) {
                    image.SetActive(true);
                }
            }
            
            if (dingSound) {
                dingSound.Play();
            }
            // platformResult.enabled = true;
            confirmButton.SetActive(true);
            flashingAlert = null;
            invalidCombo.enabled = false;
        }
        else {
            // platformResult.enabled = false;
            confirmButton.SetActive(false);
            flashingAlert = StartCoroutine(FlashingAlert(0.150f));
        }
    }

    public void ClearInventoryResult() {
        resultCounterText.text = structureCreator.counterCreated.ToString() + "/5";
        foreach(GameObject image in images) {
            image.SetActive(false);
        }
        flashingAlert = null;
        invalidCombo.enabled = false;
    }

    IEnumerator FlashingAlert(float rate) {
        if (alertSound) {
            alertSound.Play();
        }
        yield return new WaitForSeconds(rate);
        invalidCombo.enabled = false;
        yield return new WaitForSeconds(rate);
        invalidCombo.enabled = true;
        yield return new WaitForSeconds(rate);
        invalidCombo.enabled = false;
        yield return new WaitForSeconds(rate);
        invalidCombo.enabled = true;
        yield return new WaitForSeconds(rate);
        invalidCombo.enabled = false;
        yield return new WaitForSeconds(rate);
        invalidCombo.enabled = true;
    }

    
}
