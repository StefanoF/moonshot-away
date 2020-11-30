using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "StructureState", order = 1)]
public class StructureState : ScriptableObject
{
    public enum StructureStates {Empty, Full, HasItems};
    public StructureStates currentState;
    public List<GameObject> inventory;
    public int capacity;

    public void Reset() {
        inventory = new List<GameObject>();
        currentState = StructureStates.Empty;
    }

    public void AddItem(GameObject item) {
        if (inventory.Count < capacity) {
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
            if (obj.GetComponent<Interact>().interactName == name) {
                count += 1;
            }
        }
        return count;
    }

    public void setEmpty() {
        currentState = StructureStates.Empty;
    }

    public bool checkEmpty() {
        return currentState == StructureStates.Empty;
    }

    public void setFull() {
        currentState = StructureStates.Full;
    }

    public bool checkFull() {
        return currentState == StructureStates.Full;
    }

    public void setHasItems() {
        currentState = StructureStates.HasItems;
    }

    public bool checkHasItems() {
        return currentState == StructureStates.HasItems;
    }
}
