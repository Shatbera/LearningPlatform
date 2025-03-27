using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [field: SerializeField] public IPopupsController.PopupTag PopupTag;
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
        animator.SetBool("Open", true);
        gameObject.SetActive(showCloseBtn);
    }

    public void Close()
    {
        animator.SetBool("Open", false);
        closeBtn.gameObject.SetActive(false);
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
