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

    private Vector2 _dragStartPos;
    private bool _previewShown = false;
    private const float DRAG_Y = 50f;

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
        _dragStartPos = eventData.position;
        _previewShown = false;
        _dragContainer.SetParent(_canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _dragContainer.anchoredPosition += eventData.delta;

        float dragDeltaY = eventData.position.y - _dragStartPos.y;

        if (!_previewShown && dragDeltaY > DRAG_Y)
        {
            _researchPadRef.Service.TryShowSamplePreview(_sampleEntry.Item);
            _previewShown = true;
        }
        else if (_previewShown && dragDeltaY <= DRAG_Y)
        {
            _researchPadRef.Service.TryHideSamplePreview(_sampleEntry.Item);
            _previewShown = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float dragDeltaY = eventData.position.y - _dragStartPos.y;

        if (_previewShown)
        {
            _researchPadRef.Service.TryHideSamplePreview(_sampleEntry.Item);
            _previewShown = false;
        }

        if (dragDeltaY > DRAG_Y)
        {
            if (_researchPadRef.Service.TryPlaceSample(_sampleEntry.Item))
            {
                SetVisible(false);
            }
        }

        _dragContainer.SetParent(transform);
        _dragContainer.localPosition = Vector2.zero;
    }
}
