using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenUI : MonoBehaviour
{   
    public GameObject actives;
    public PlayerState playerState;
    public Image[] steps;
    private Image[] reversedSteps;


    [Range(0f, 1f)]
    public float percent;

    [Range(0f, 5f)]
    public float ratioEverySeconds;

    [Range(0f, 0.1f)]
    public float ratioDecrease;

    private int percentRatio;
    private float timer;
    private Coroutine blinkingCo;

    private void Awake() {
        reversedSteps = new Image[steps.Length];
        for (int i = 0; i < steps.Length; i++) {
            reversedSteps[ steps.Length - 1 - i ] = steps[ i ];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerState.isStoryEnded) {
            return;
        }
        timer += Time.deltaTime;
        if (timer > ratioEverySeconds && percent > 0f) 
        {
            percent -= ratioDecrease;
            if (percent < 0f) {
                percent = 0f;
            }
            timer = 0f;
            if (!playerState.isInRocket && !playerState.isDie) {
                UpdateSteps();
            }
        }
    }

    void UpdateSteps() {
        print("OxygenUI UpdateSteps");
        percentRatio = (int) (percent * reversedSteps.Length);

        if (percentRatio == 0) {
            playerState.playerDie.Raise();
            return;
        }

        if (percentRatio <= 3) {
            if (blinkingCo != null) {
                StopCoroutine(blinkingCo);
            }
            blinkingCo = StartCoroutine(BlinkingBar());
        }

        for (int i = 0; i < reversedSteps.Length; i++) {
            if (i >= percentRatio) {
                reversedSteps[i].enabled = false;
            }
            else {
                reversedSteps[i].enabled = true;
            }
        }
    }

    IEnumerator BlinkingBar() {
        yield return new WaitForSeconds(0.5f);
        actives.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        actives.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        actives.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        actives.SetActive(true);
    }
}
