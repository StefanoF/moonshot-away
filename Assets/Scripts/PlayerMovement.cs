using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public AudioSource looseSound;
    public GameObject loosePanel;
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float playerSpeed;
    // private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    public Animator anim;

    public PlayerState playerState;
    private Vector3 lastPosition;
    

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    void Update()
    {
        playerState.setGrounded(controller.isGrounded);
        if (playerState.checkGrounded() && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (!playerState.isDie) {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
        else {
            playerState.setIdle();
            gameObject.transform.position = lastPosition;
        }

        if (lastPosition != transform.position) {
            playerState.setRunning();
            anim.SetBool("Running", true);
        } else {
            if (!playerState.checkTaking()) {
                playerState.setIdle();
                anim.SetBool("Taking", false);
                anim.SetBool("Running", false);
            }
        }
        lastPosition = gameObject.transform.position;
    }

    public void GameOverDie() {
        print("Game over");
        playerState.isDie = true;
        anim.SetTrigger("Die");
        StartCoroutine(ShowLoosePanel());
    }

    IEnumerator ShowLoosePanel() {
        yield return new WaitForSeconds(1f);
        looseSound.Play();
        loosePanel.SetActive(true);
    }

}
