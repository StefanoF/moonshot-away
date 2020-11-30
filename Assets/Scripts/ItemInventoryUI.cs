using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemInventoryUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public PlayerState playerState;

    public void OnPointerClick(PointerEventData pointerEventData) {
        if (gameObject.transform.parent.gameObject.name.StartsWith("Player")) {
            ClickOnPlayerInventory();
        }
        else {
            ClickOnStructureInventory();
        }
    }

    private void ClickOnPlayerInventory() {
        if (!playerState.currentInteract) {
            playerState.helpText = "Item released on the ground!";
            playerState.updateHelpText.Raise();
            playerState.clickedObjName = gameObject.name.Substring("Image_".Length);
            playerState.playerReleaseItem.Raise();
            return;
        }
        if (!playerState.isInStructure) {
            return;
        }

        StructureCreator structureCreator = playerState.currentInteract.GetComponent<StructureCreator>();
        if (structureCreator.structureState.checkFull()) {
            playerState.helpText = "The inventory of this structure is full!";
            playerState.updateHelpText.Raise();
            return;
        }

        GameObject itemFromInventory = playerState.findItemByName(gameObject.name.Substring("Image_".Length));
        if (!itemFromInventory) {
            print("PlayerItemUI: itemFromInventory non trovato");
            return;
        }

        playerState.RemoveItem(itemFromInventory);
        structureCreator.structureState.AddItem(itemFromInventory);
        playerState.updateInventoryUI.Raise();
    }

    private void ClickOnStructureInventory() {
        if (!playerState.currentInteract || !playerState.isInStructure) {
            print("!playerState.currentInteract");
            return;
        }
        StructureCreator structureCreator = playerState.currentInteract.GetComponent<StructureCreator>();

        if (playerState.checkFull()) {
            playerState.helpText = "Your inventory is full!";
            playerState.updateHelpText.Raise();
            return;
        }

        GameObject itemFromInventory = structureCreator.structureState.findItemByName(gameObject.name.Substring("Image_".Length));
        if (!itemFromInventory) {
            print("PlayerItemUI: itemFromInventory non trovato");
            return;
        }

        playerState.AddItem(itemFromInventory);
        structureCreator.structureState.RemoveItem(itemFromInventory);
        playerState.updateInventoryUI.Raise();
    }

    public void OnPointerExit(PointerEventData eventData) {
    }

    public void OnPointerEnter(PointerEventData eventData) {
        
    }
}