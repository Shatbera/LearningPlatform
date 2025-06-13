using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour
{
    [SerializeField] private ResearchSampleSystemServiceRefSO _samplesRefSO;
    [SerializeField] private ComponentPool<InventoryItemSlot> _inventorySlots;

    //[SerializeField] private TMP_Text _itemNameTxt;
    //[SerializeField] private TMP_Text _itemInfoTxt;
    [SerializeField] private LocalizedTextSetter _itemNameTxtSetter;
    [SerializeField] private LocalizedTextSetter _itemInfoTxtSetter;
    [SerializeField] private Image _itemIconImg;

    private List<InventoryItemSlot> _activeSlots = new();
    private void Awake()
    {
        GetComponent<Popup>().Opened += Setup;
    }

    private void Setup()
    {
        CleanUp();

        CollectableItemEntry<ResearchSampleSO, ResearchSampleItemState>[] items = _samplesRefSO.Service.GetAll()
            .Where(x => x.State.Amount > 0)
            .OrderByDescending(x => x.State.IsUnlocked)
            .ToArray();
        foreach(var item in items)
        {
            var slot = _inventorySlots.Get();
            slot.Setup(item, OnItemSelected);
            slot.SetSelected(false);
            slot.transform.SetParent(_inventorySlots.Parent);
            _activeSlots.Add(slot);
        }
        //_itemNameTxt.text = "...";
        //_itemInfoTxt.text = "...";
        _itemNameTxtSetter.SetRawText("...");
        _itemInfoTxtSetter.SetRawText("...");
        _itemIconImg.gameObject.SetActive(false);
    }

    private void OnItemSelected(InventoryItemSlot itemSlot)
    {
        foreach(var slot in _activeSlots)
        {
            slot.SetSelected(slot == itemSlot);
        }
        //_itemNameTxt.text = itemSlot.Item.Item.DisplayName;
        //_itemInfoTxt.text = itemSlot.Item.State.IsUnlocked ? itemSlot.Item.Item.SampleInfo : "Locked";
        _itemNameTxtSetter.SetLocalizedText(itemSlot.Item.Item.LocalizedDisplayName);
        if (itemSlot.Item.State.IsUnlocked)
        {
            _itemInfoTxtSetter.SetLocalizedText(itemSlot.Item.Item.LocalizedSampleInfo);
        }
        else
        {
            _itemInfoTxtSetter.SetRawText("???");
        }
        _itemIconImg.sprite = itemSlot.Item.Item.Icon;
        _itemIconImg.gameObject.SetActive(true);
    }

    private void CleanUp()
    {
        foreach(var item in _activeSlots)
        {
            _inventorySlots.Release(item);
            item.transform.SetParent(transform);
        }
        _activeSlots.Clear();
    }
}
