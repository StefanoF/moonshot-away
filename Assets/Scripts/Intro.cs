using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public PlayerState playerState;
    public Text introText;
    public float textRatio;
    public float fadeSpeed;

    [Header("Moon movement")]
    public RectTransform rectTransform;
    public float moonSpeed;


    private string[] story;
    private int currentStoryIndex;
    private Coroutine mainCo;
    private Coroutine co;
    private enum FadeDirection
    {
        In, //Alpha = 1
        Out // Alpha = 0
    }

    private void Awake() {
        story = new string[6];
        story[0] = "You are lost in the moon...";
        story[1] = "Oxygen is visibly decreasing...";
        story[2] = "but...";
        story[3] = "but you remember how things fit together!";
        story[4] = "Try to construct something useful...";
        story[5] = "...to go away from here!";
        introText.text = "";
    }

    private void Start() {
        AddStoryline();
        
    }

    private void Update() {
        if (playerState.isStoryEnded) {
            return;
        }
        MoveMoon();
    }

    public void MoveMoon() {
        Vector2 position = rectTransform.anchoredPosition;
        position.x += moonSpeed * Time.deltaTime;
        rectTransform.anchoredPosition = position;
        rectTransform.Rotate (rectTransform.forward * moonSpeed * 0.01f);
    }

    public void SkipStory() {
        StopCoroutine(mainCo);
        StopCoroutine(co);
        playerState.isStoryEnded = true;
        gameObject.SetActive(false);
    }

    private void AddStoryline(int index = 0) {
        if (mainCo != null) {
            StopCoroutine(mainCo);
        }

        if (index < story.Length) {
            mainCo = StartCoroutine(Storyline(story[index]));
        }
        else {
            SkipStory();
        }
    }

    private IEnumerator Storyline(string text) {
        introText.text = text;
        Fade(FadeDirection.In);
        yield return new WaitForSeconds(textRatio);
        yield return new WaitForSeconds(textRatio);
        Fade(FadeDirection.Out);
        yield return new WaitForSeconds(textRatio);
        currentStoryIndex += 1;
        AddStoryline(currentStoryIndex);
    }

    private void Fade(FadeDirection fadeDirection) {
        if (co != null) {
            StopCoroutine(co);
        }
        co = StartCoroutine(FadeText(fadeDirection));
    }

    private IEnumerator FadeText(FadeDirection fadeDirection) 
    {
        float alpha = (fadeDirection == FadeDirection.Out)? 1 : 0;
        float fadeEndValue = (fadeDirection == FadeDirection.Out)? 0 : 1;
        if (fadeDirection == FadeDirection.Out) {
            while (alpha >= fadeEndValue)
            {
                SetColorText (ref alpha, fadeDirection);
                yield return null;
            }
            introText.enabled = false; 
        } else {
            introText.enabled = true; 
            while (alpha <= fadeEndValue)
            {
                SetColorText (ref alpha, fadeDirection);
                yield return null;
            }
        }
    }

    private void SetColorText(ref float alpha, FadeDirection fadeDirection)
    {
        introText.color = new Color (introText.color.r,introText.color.g, introText.color.b, alpha);
        alpha += Time.deltaTime * (1.0f / fadeSpeed) * ((fadeDirection == FadeDirection.Out)? -1 : 1) ;
    }


}
