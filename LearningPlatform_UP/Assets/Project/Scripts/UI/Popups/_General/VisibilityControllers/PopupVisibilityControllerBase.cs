using UnityEngine;

public abstract class PopupVisibilityControllerBase : MonoBehaviour
{
    public virtual void Initialize(Popup popup) { }
    public abstract void SetVisible(Popup popup, bool visible);
    public virtual void Dispose(Popup popup) { }
}
