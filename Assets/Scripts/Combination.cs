using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combination : MonoBehaviour
{
    public string[] combo;
    public GameObject result;

    public bool ValidateCombo(GameObject[] proposedCombo) {
        if (proposedCombo.Length != combo.Length) {
            return false;
        }

        List<string> validsFound = new List<string>();
        foreach (GameObject proposedObj in proposedCombo) {
            foreach (string obj in combo) {
                string interactName = proposedObj.GetComponent<Interact>().interactName;
                if (interactName == obj && !validsFound.Contains(interactName)) {
                    validsFound.Add(interactName);
                }
            }
        }
        return validsFound.Count == combo.Length;
    }
}
