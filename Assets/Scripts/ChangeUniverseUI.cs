using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeUniverseUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject tooltip;
    public PlayerState playerState;

    public void OnPointerClick(PointerEventData pointerEventData) {
        playerState.changeUniverse.Raise();
    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        tooltip.SetActive(true);
    }
}
