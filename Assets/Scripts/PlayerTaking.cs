using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTaking : MonoBehaviour
{
    public PlayerState playerState;
    public Animator anim;

    [Range(0.01f, 10f)]
    public float detectRadius = 0.01f;
    public Transform detectCollider;

    private Coroutine tidCoroutine;

    // Update is called once per frame
    void Update()
    {
        InteractItem();
    }

    private void InteractItem() {
        if (Input.GetButtonDown("Jump") && playerState.checkGrounded() && playerState.checkIdle()) {
            playerState.setTaking();
            anim.SetBool("Taking", playerState.checkTaking());
            
            if (playerState.currentInteract) {
                tidCoroutine = StartCoroutine(InteractItemDelayed(0.75f));
            }
        }

        if (tidCoroutine != null && !playerState.checkTaking()) {
            StopCoroutine(tidCoroutine);
        }
    }

    IEnumerator InteractItemDelayed(float time) {
        yield return new WaitForSeconds(time);

        if (playerState.currentInteract.tag == "Takeable") {
            TakeItem(playerState.currentInteract);
        }
        else if (playerState.currentInteract.tag == "ReleaseArea") {
            GiveItem(playerState.currentInteract);
        }
        else if (playerState.currentInteract.tag == "RocketPiece") {
            MakeRocketPiece();
        }
        else if (playerState.currentInteract.tag == "Rocket") {
            LaunchRocket();
        }

        yield return new WaitForSeconds(3.26f);
        playerState.setIdle();
        anim.SetBool("Taking", false);
    }

    private void TakeItem(GameObject objectTook) {
        if (objectTook) {
            if (playerState.checkFull()) {
                playerState.helpText = "Your inventory is full!";
                playerState.updateHelpText.Raise();
                return;
            }
            playerState.AddItem(objectTook);
            playerState.updateInventoryUI.Raise();
            objectTook.SetActive(false);
            playerState.currentInteract = null;
        }
    }

    private void GiveItem(GameObject structure) {
        if (structure) {
            playerState.isInStructure = true;
        }
        
        if (structure.name == "PlatformStructure") {
            playerState.playerInPlatform.Raise();
            playerState.updateInventoryUI.Raise();
        }
        if (structure.name == "RocketStructure") {
            playerState.playerInRocket.Raise();
            playerState.updateInventoryUI.Raise();
        }
        if (structure.name == "HelperStructure") {
            playerState.playerInHelper.Raise();
            playerState.updateInventoryUI.Raise();
        }
    }

    public void ReleaseItem() {
        if (playerState.clickedObjName.Length == 0) {
            print("PlayerTaking: ReleaseItem clickedObjName non settato");
            return;
        }
        GameObject itemFromInventory = playerState.findItemByName(playerState.clickedObjName);
        if (!itemFromInventory) {
            print("PlayerTaking: ReleaseItem non trovato");
            return;
        }

        playerState.RemoveItem(itemFromInventory);
        playerState.updateInventoryUI.Raise();

        itemFromInventory.transform.position = new Vector3(
            detectCollider.position.x + 1f, 
            2f, 
            detectCollider.position.z + 1f
        );
        itemFromInventory.SetActive(true);
    }

    public void MakeRocketPiece() {
        playerState.resultCreated.Raise();
    }

    public void LaunchRocket() {
        playerState.isInRocket = true;
        playerState.launchRocket.Raise();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (detectCollider.position, detectRadius);
    }
}
