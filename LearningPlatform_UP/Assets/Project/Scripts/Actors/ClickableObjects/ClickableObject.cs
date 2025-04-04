using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public abstract void OnClick();
}
