using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _iconImg;
    [SerializeField] private GameObject _selectedOutline;
    public CollectableItemEntry<ResearchSampleSO, ResearchSampleItemState> Item { get; private set; }
    private Action<InventoryItemSlot> _selectAction;

    public void Setup(CollectableItemEntry<ResearchSampleSO, ResearchSampleItemState> item, Action<InventoryItemSlot> onSelect)
    {
        Item = item;
        _selectAction = onSelect;
        _iconImg.sprite = item.Item.Icon;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _selectAction?.Invoke(this);
    }

    public void SetSelected(bool selected)
    {
        _selectedOutline.SetActive(selected);
    }

    private void OnDisable()
    {
        _selectAction = null;
    }
}
