using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [field: SerializeField] public IPopupsController.PopupTag PopupTag;
    [field: SerializeField] public bool CloseWithTint;
    [SerializeField] private bool showCloseBtn = true;

    [SerializeField] private Animator animator;
    [SerializeField] private Button closeBtn;

    private void Start()
    {
        gameObject.SetActive(false);
        closeBtn.gameObject.SetActive(false);
        closeBtn.onClick.AddListener(Close);
    }
    public void Open()
    {
        PopupsController.Instance.OpenPopup(this);
    }

    public void Close()
    {
        PopupsController.Instance.ClosePopup(this);
    }

    public void SetVisible(PopupsController controller, bool visible)
    {
        if (visible)
        {
            gameObject.SetActive(true);
        }
        animator.SetBool("Open", visible);
        closeBtn.gameObject.SetActive(visible && showCloseBtn);
    }
    public void OnPopupOpenFinished()
    {
        closeBtn.gameObject.SetActive(true);
    }

    public void OnPopupCloseFinished()
    {
        gameObject.SetActive(false);
    }
}
