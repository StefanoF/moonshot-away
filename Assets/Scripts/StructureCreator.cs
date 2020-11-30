using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureCreator : MonoBehaviour
{
    public StructureState structureState;
    public PlayerState playerState;
    public List<Combination> validCombinations;    
    public GameObject lastComboResult;
    public List<string> alreadyCreated;
    public int counterCreated;

    private void Start() {
        lastComboResult = null;
        alreadyCreated = new List<string>();
    }

    public void Generate() {
        if (lastComboResult) {
            print("StructureCreator Generate: " + lastComboResult.name);

            if (lastComboResult.name == "changeUniverse") {
                playerState.changeUniverse.Raise();
            }
            else {
                if (lastComboResult.name.StartsWith("platform_")) {
                    playerState.helpText = "A new platform piece created, keep it up!";
                }
                else {
                    playerState.helpText = "A new rocket piece created, good!";
                }
                playerState.updateHelpText.Raise();
                alreadyCreated.Add(lastComboResult.name);
                lastComboResult.SetActive(true);

            }
            counterCreated += 1;
            structureState.ClearInventory();
            lastComboResult = null;
        }
    }

    public GameObject VerifyCombination() {
        if (structureState.checkFull()) {
            foreach(Combination combo in validCombinations) {
                if (combo.ValidateCombo(structureState.inventory.ToArray())) {
                    if (!alreadyCreated.Contains(combo.result.name)) {
                        lastComboResult = combo.result;
                        return combo.result;
                    }
                }
            }
        }
        return null;
    }
}
