public class FullScreenPanelVisibilityController : PopupVisibilityControllerBase
{
    public override void SetVisible(Popup popup, bool visible)
    {
        popup.gameObject.SetActive(visible);       
    }
}
