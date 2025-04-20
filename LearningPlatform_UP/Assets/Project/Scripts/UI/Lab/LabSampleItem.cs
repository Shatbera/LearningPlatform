using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LabSampleItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _iconImg;
    [SerializeField] private RectTransform _dragContainer;

    private CollectableItemEntry<ResearchSampleSO> _sampleEntry;
    private RectTransform _canvasRectTransform;
    private RectTransform _dragRect;
    private Vector2 _offset;
    private Transform _originalParent;
    private Vector2 _originalPosition;
    private Canvas _canvas;

    private void Awake()
    {
        _dragRect = _dragContainer;
        _canvas = GetComponentInParent<Canvas>();
        _canvasRectTransform = _canvas.GetComponent<RectTransform>();
    }

    public void Setup(CollectableItemEntry<ResearchSampleSO> entry)
    {
        _sampleEntry = entry;
        _iconImg.sprite = entry.Item.Icon;
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalParent = _dragRect.parent;
        _originalPosition = _dragRect.anchoredPosition;

        _dragRect.SetParent(_canvas.transform);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform, eventData.position, _canvas.worldCamera, out Vector2 localPointerPosition);

        _offset = _dragRect.anchoredPosition - localPointerPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform, eventData.position, _canvas.worldCamera, out Vector2 localPointerPosition))
        {
            _dragRect.anchoredPosition = localPointerPosition + _offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragRect.SetParent(_originalParent);
        _dragRect.anchoredPosition = _originalPosition;
    }
}
