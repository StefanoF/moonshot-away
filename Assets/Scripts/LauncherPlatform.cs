using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LauncherPlatform : MonoBehaviour
{
    public List<GameObject> pieces;
    public PlayerState playerState;

    public BoxCollider mainCollider;
    public Interact interactScript;
    public GameObject player;
    public GameObject winnerPanel;

    [Header("Countdown")]
    public Text countdown;
    public bool started;
    private float rocketSpeed;
    public float speedRatio;
    public Transform startPos;
    public Transform endPos;
    public GameObject targetRocket;
    
    public AudioSource winnerSound;
    public AudioSource countDownSound;
    public AudioSource alertSound;
    public AudioSource piecePlacedSound;

    [Header("Rocket Engines")]
    public ParticleSystem flame;
    public ParticleSystem fog;

    public PlayerFollow playerFollow;

    public AudioSource engine;

    private Coroutine launchCo;
    private float t;


    public void UpdateLauncher() {
        if (!playerState.currentInteract) {
            print("UpdateLauncher: currentInteract non trovato");
            return;
        }

        List<GameObject> inactivePieces = pieces.Where(p => p.activeSelf == false).ToList();

        if (inactivePieces.Count == 0) {
            playerState.helpText = "All of the rocket pieces, are already placed!";
            playerState.updateHelpText.Raise();
            playerState.currentInteract = null;
            return;
        }
        if (playerState.currentInteract.name == inactivePieces[0].name) {
            piecePlacedSound.Play();
            playerState.currentInteract.SetActive(false);
            inactivePieces[0].SetActive(true);
            playerState.helpText = "Launcher Platform: congrats, piece placed correctly!";
        }
        else {
            alertSound.Play();
            playerState.helpText = "Launcher Platform: sorry, put another piece before this!";
            playerState.updateHelpText.Raise();
        }

        playerState.currentInteract = null;

        if (inactivePieces.Count == 1) {
            mainCollider.enabled = true;
            interactScript.enabled = true;
        }
    }

    public void Launch() {
        player.SetActive(false);
        interactScript.enabled = false;
        playerFollow.playerTransform = targetRocket.transform;

        if (launchCo != null) {
            StopCoroutine(launchCo);
        }
        launchCo = StartCoroutine(Countdown(1f));
    }

    IEnumerator Countdown(float time) {
        StartEngines();
        countdown.enabled = true;
        yield return new WaitForSeconds(time);
        countdown.text = "9";
        yield return new WaitForSeconds(time);
        countdown.text = "8";
        yield return new WaitForSeconds(time);
        countdown.text = "7";
        yield return new WaitForSeconds(time);
        countdown.text = "6";
        yield return new WaitForSeconds(time);
        countdown.text = "5";
        yield return new WaitForSeconds(time);
        countdown.text = "4";
        yield return new WaitForSeconds(time);
        countdown.text = "3";
        yield return new WaitForSeconds(time);
        countdown.text = "2";
        flame.Play();
        yield return new WaitForSeconds(time);
        countdown.text = "1";
        fog.Stop();
        yield return new WaitForSeconds(time);
        countdown.enabled = false;
        started = true;
    }

    private void StartEngines() {
        engine.Play();
        fog.Play();
        countDownSound.Play();
    }

    private void Update() {
        if (started) {
            rocketSpeed = rocketSpeed + speedRatio;
            t += Time.deltaTime * rocketSpeed;
            targetRocket.transform.position = Vector3.Lerp(startPos.position, endPos.position, t);
            if (targetRocket.transform.position == endPos.position) {
                winnerSound.Play();
                winnerPanel.SetActive(true);
                started = false;
            }
        }
    }
}
