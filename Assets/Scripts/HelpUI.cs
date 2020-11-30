using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HelpUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{   
    private bool isVisible;
    public GameObject help;
    public GameObject tooltip;
    public Image iconImage;

    void Awake() {
        iconImage = gameObject.GetComponent<Image>();
    }

    void Update() {
        if(Input.GetKey(KeyCode.Escape) && isVisible){
            HideHelp();
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData) {
        if (isVisible) {
            HideHelp();
        }
        else {
            ShowHelp();
        }
    }

    void ShowHelp() {
        iconImage.color = new Color32 (31,31,31,255);
        isVisible = true;
        Time.timeScale = 0;
        help.SetActive(true);
    }

    void HideHelp() {
        iconImage.color = new Color32 (255,255,255,255);
        isVisible = false;
        Time.timeScale = 1;
        help.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        tooltip.SetActive(true);
    }
}
