using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PopupTintController : MonoBehaviour, IPointerClickHandler
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false);
    }
    public void ShowTint()
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, 0.15f);
    }

    public void HideTint()
    {
        _canvasGroup.DOFade(0, 0.15f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(PopupsController.Instance.TopPopup != null && PopupsController.Instance.TopPopup.CloseWithTint)
        {
            PopupsController.Instance.CloseTopPopup();
        }
    }
}
