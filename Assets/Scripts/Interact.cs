using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public PlayerState playerState;
    public string interactName;

    public GameObject keyFollowObject;

    void OnDisable()
    {
        if (keyFollowObject) {
            keyFollowObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        if (keyFollowObject) {
            keyFollowObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") || gameObject.CompareTag("ReleaseArea")) {
            playerState.currentInteract = gameObject;
            if (keyFollowObject) {
                keyFollowObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player") || gameObject.CompareTag("ReleaseArea")) {
            playerState.currentInteract = null;
            if (keyFollowObject) {
                keyFollowObject.SetActive(false);
            }

            if (gameObject.CompareTag("ReleaseArea")) {
                playerState.isInStructure = false;
                if (gameObject.name == "PlatformStructure") {
                    playerState.playerOutPlatform.Raise();
                }
                if (gameObject.name == "RocketStructure") {
                    playerState.playerOutRocket.Raise();
                }
                if (gameObject.name == "HelperStructure") {
                    playerState.playerOutHelper.Raise();
                }
            }
        }
    }
    
}
