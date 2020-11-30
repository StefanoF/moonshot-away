using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "PlayerState", order = 1)]
public class PlayerState : ScriptableObject
{
    public enum PlayerStates {Idle, Running, Taking};
    public enum InventoryStates {Empty, Full, HasItems};

    public bool isDie;
    public bool isGrounded;
    public bool isInRocket;
    public PlayerStates currentState;
    public InventoryStates inventoryState;

    public List<GameObject> inventory;

    public int capacity;

    public GameObject currentInteract;
    public bool isInStructure;
    public string clickedObjName;
    public string helpText;
    public bool isStoryEnded;

    [Header("Events")]
    public GameEvent updateInventoryUI;
    public GameEvent playerDie;
    public GameEvent playerInPlatform;
    public GameEvent playerOutPlatform;
    public GameEvent playerInRocket;
    public GameEvent playerOutRocket;
    public GameEvent playerInHelper;
    public GameEvent playerOutHelper;
    public GameEvent playerReleaseItem;
    public GameEvent resultCreated;
    public GameEvent updateHelpText;
    public GameEvent changeUniverse;
    public GameEvent launchRocket;

    public void Reset() {
        isGrounded = false;
        inventory = new List<GameObject>();
        currentState = PlayerStates.Idle;
        inventoryState = InventoryStates.Empty;
        currentInteract = null;
        isInStructure = false;
        clickedObjName = "";
        helpText = "";
        isDie = false;
        isInRocket = false;
        isStoryEnded = false;
    }

    public void AddItem(GameObject item) {
        if (inventory.Count < capacity && !inventory.Contains(item)) {
            inventory.Add(item);

            if (inventory.Count == capacity) {
                setFull();
            } else {
                setHasItems();
            }
        }
    }

    public void RemoveItem(GameObject item) {
        if (inventory.Count > 0) {
            inventory.Remove(item);

            if (inventory.Count == 0) {
                setEmpty();
            } else {
                setHasItems();
            }
        }
    }

    public void ClearInventory() {
        if (inventory.Count > 0) {
            inventory.Clear();
            setEmpty();
        }
    }

    public GameObject findItemByName(string name) {
        GameObject item = null;
        foreach(GameObject obj in inventory) {
            if (obj.GetComponent<Interact>().interactName == name) {
                item = obj;
                break;
            }
        }
        return item;
    }

    public int countItemsByName(string name) {
        int count = 0;
        foreach(GameObject obj in inventory) {
            Debug.Log("obj.GetComponent<Interact>().interactName: " + obj.GetComponent<Interact>().interactName);
            Debug.Log("countItemsByName name: " + name);
            if (obj.GetComponent<Interact>().interactName == name) {
                count += 1;
            }
        }
        Debug.Log("countItemsByName count: " + count.ToString());
        return count;
    }

    public GameObject GetFirstItem() {
        GameObject firstItem = null;
        foreach(GameObject obj in inventory) {
            if (!firstItem) {
                firstItem = obj;
            }
        }
        return firstItem;
    }

    public void setEmpty() {
        inventoryState = InventoryStates.Empty;
    }

    public bool checkEmpty() {
        return inventoryState == InventoryStates.Empty;
    }

    public void setHasItems() {
        inventoryState = InventoryStates.HasItems;
    }

    public bool checkHasItems() {
        return inventoryState == InventoryStates.HasItems;
    }

    public void setFull() {
        inventoryState = InventoryStates.Full;
    }

    public bool checkFull() {
        return inventoryState == InventoryStates.Full;
    }

    public void setIdle() {
        currentState = PlayerStates.Idle;
    }

    public bool checkIdle() {
        return currentState == PlayerStates.Idle;
    }

    public void setRunning() {
        currentState = PlayerStates.Running;
    }

    public bool checkRunning() {
        return currentState == PlayerStates.Running;
    }

    public void setTaking() {
        currentState = PlayerStates.Taking;
    }

    public bool checkTaking() {
        return currentState == PlayerStates.Taking;
    }

    public void setGrounded(bool grounded) {
        isGrounded = grounded;
    }

    public bool checkGrounded() {
        return isGrounded;
    }
}
