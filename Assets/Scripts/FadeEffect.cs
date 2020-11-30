using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    
    public Image fadeOutUIImage;
    public float fadeSpeed = 0.8f; 

    private Coroutine co;
 
    public enum FadeDirection
    {
        In, //Alpha = 1
        Out // Alpha = 0
    }

    private void Start() {
        DisableFade();
    }
    
    public void EnableFade() {
        if (co != null) {
            StopCoroutine(co);
        }
        co = StartCoroutine(Fade(FadeDirection.In, FadeDirection.Out));
    }

    public void DisableFade() {
        if (co != null) {
            StopCoroutine(co);
        }
        co = StartCoroutine(Fade(FadeDirection.Out));
    }

    private IEnumerator Fade(FadeDirection fadeDirection, FadeDirection fadeDirection1 = FadeDirection.In) 
    {
        float alpha = (fadeDirection == FadeDirection.Out)? 1 : 0;
        float fadeEndValue = (fadeDirection == FadeDirection.Out)? 0 : 1;
        if (fadeDirection == FadeDirection.Out) {
            while (alpha >= fadeEndValue)
            {
                SetColorImage (ref alpha, fadeDirection);
                yield return null;
            }
            fadeOutUIImage.enabled = false; 
        } else {
            fadeOutUIImage.enabled = true; 
            while (alpha <= fadeEndValue)
            {
                SetColorImage (ref alpha, fadeDirection);
                yield return null;
            }
        }

        if (fadeDirection1 == FadeDirection.Out) {
            fadeOutUIImage.color = new Color (fadeOutUIImage.color.r,fadeOutUIImage.color.g, fadeOutUIImage.color.b, 1);
            yield return new WaitForSeconds(1f);

            co = StartCoroutine(Fade(FadeDirection.Out));
            yield return null;
        }
    }
 
    private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
    {
        fadeOutUIImage.color = new Color (fadeOutUIImage.color.r,fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
        alpha += Time.deltaTime * (1.0f / fadeSpeed) * ((fadeDirection == FadeDirection.Out)? -1 : 1) ;
    }
}