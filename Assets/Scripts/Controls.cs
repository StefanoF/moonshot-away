using System.Collections;
using UnityEngine;

public class Controls : MonoBehaviour
{
    private Coroutine co;
    public PlayerState playerState;
    private bool isHided;

    private void Update() {
        if (playerState.isStoryEnded && !isHided) {
            if (co != null) {
                StopCoroutine(co);
            }

            co = StartCoroutine(HideControls());
            isHided = true;
        }
    }


    private IEnumerator HideControls() 
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        yield return null;
    }
}
