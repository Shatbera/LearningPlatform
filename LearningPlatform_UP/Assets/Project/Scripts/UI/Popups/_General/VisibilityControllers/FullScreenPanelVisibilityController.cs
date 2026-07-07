using UnityEngine;

public class FullScreenPanelVisibilityController : PopupVisibilityControllerBase
{
    [SerializeField] private GameObject _container;
    public override void SetVisible(Popup popup, bool visible)
    {
        _container.SetActive(visible);
    }
}
