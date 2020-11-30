using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HelpTextUI : MonoBehaviour
{   
    public PlayerState playerState;
    private Text helpTextBottom;
    private Coroutine co;

    private void Awake() {
        helpTextBottom = GetComponent<Text>();
    }

    public void UpdateHelpText() {
        if (co != null) {
            StopCoroutine(co);
        }
        helpTextBottom.text = playerState.helpText;
        co = StartCoroutine(DelayedClear(3f));
    }

    IEnumerator DelayedClear(float time) {
        yield return new WaitForSeconds(time);
        helpTextBottom.text = "";
        playerState.helpText = "";
    }
}