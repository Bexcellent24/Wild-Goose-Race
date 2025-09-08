using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UnityEngine.UI.Selectable))]
public class UIButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        SFXManager.Instance?.PlayGlobalSound("Hover", .5f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SFXManager.Instance?.PlayGlobalSound("Button", .75f);
    }
}