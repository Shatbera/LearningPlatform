using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LabSampleItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _iconImg;
    [SerializeField] private RectTransform _dragContainer;

    [SerializeField] private ResearchPadRefSO _researchPadRef;

    private CollectableItemEntry<ResearchSampleSO, ResearchSampleItemState> _sampleEntry;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
    }

    public void Setup(CollectableItemEntry<ResearchSampleSO, ResearchSampleItemState> entry)
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
        _dragContainer.SetParent(_canvas.transform);
        _researchPadRef.Service.TryShowSamplePreview(_sampleEntry.Item);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _dragContainer.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _researchPadRef.Service.TryHideSamplePreview(_sampleEntry.Item);
        if (_researchPadRef.Service.TryPlaceSample(_sampleEntry.Item))
        {
            _sampleEntry.ChangeAmount(-1);
            SetVisible(false);
        }
        _dragContainer.SetParent(transform);
        _dragContainer.localPosition = Vector2.zero;
    }
}
