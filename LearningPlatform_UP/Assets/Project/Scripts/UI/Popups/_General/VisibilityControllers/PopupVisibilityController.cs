using UnityEngine;
using UnityEngine.UI;

public class PopupVisibilityController : PopupVisibilityControllerBase
{
    [SerializeField] private bool showCloseBtn = true;
    [SerializeField] private Animator animator;
    [SerializeField] private Button closeBtn;

    private Popup popup;

    public override void Initialize(Popup popup)
    {
        this.popup = popup;

        popup.gameObject.SetActive(false);

        if (closeBtn != null)
        {
            closeBtn.gameObject.SetActive(false);
            closeBtn.onClick.AddListener(popup.Close);
        }
    }

    public override void SetVisible(Popup popup, bool visible)
    {
        if (visible)
        {
            popup.gameObject.SetActive(true);
        }

        animator.SetBool("Open", visible);

        if (closeBtn != null)
        {
            closeBtn.gameObject.SetActive(visible && showCloseBtn);
        }

        if (visible)
        {
            popup.NotifyOpened();
        }
        else
        {
            popup.NotifyClosed();
        }
    }

    public void OnPopupOpenFinished(Popup popup)
    {
        if (closeBtn != null)
        {
            closeBtn.gameObject.SetActive(showCloseBtn);
        }
    }

    public void OnPopupCloseFinished(Popup popup)
    {
        Popup targetPopup = popup != null ? popup : this.popup;
        if (targetPopup == null)
        {
            Debug.LogWarning($"{nameof(PopupVisibilityController)} close animation finished without a popup.", this);
            return;
        }

        targetPopup.gameObject.SetActive(false);
    }

    public override void Dispose(Popup popup)
    {
        if (closeBtn != null)
        {
            closeBtn.onClick.RemoveListener(popup.Close);
        }

        if (this.popup == popup)
        {
            this.popup = null;
        }
    }
}
