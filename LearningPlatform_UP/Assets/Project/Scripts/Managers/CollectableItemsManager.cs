using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectableItemContainer
{
    public CollectableItemSO Item;
    public int Amount;
}
public class CollectableItemsManager : MonoBehaviour
{
    private readonly Dictionary<string, CollectableItemContainer> _itemsDict = new();
    [SerializeField] private ItemCollectEventChannel _collectEventChannel;
    private void OnEnable()
    {
        _collectEventChannel.RegisterListener(RequestItemPickup);
    }


    private void OnDisable()
    {
        _collectEventChannel.UnregisterListener(RequestItemPickup);
    }

    private void RequestItemPickup(ItemCollectEventData eventData)
    {
        if(AddItem(eventData.Item.ItemSO, eventData.Amount))
        {
            eventData.Item.OnPickup();
        }
    }

    public bool AddItem(CollectableItemSO item, int amount)
    {
        if (_itemsDict.ContainsKey(item.ID))
        {
            _itemsDict[item.ID].Amount += amount;
        }
        else
        {
            _itemsDict.Add(item.ID, new CollectableItemContainer { Item = item, Amount = amount });
        }
        return true;
    }

    public CollectableItemSO TakeItem(string itemId, int amount)
    {
        if (_itemsDict.ContainsKey(itemId) && _itemsDict[itemId].Amount >= amount)
        {
            var returnItem = _itemsDict[itemId].Item;
            _itemsDict[itemId].Amount -= amount;
            if (_itemsDict[itemId].Amount == 0)
            {
                _itemsDict.Remove(itemId);
            }
            return returnItem;
        }
        return null;
    }

    public CollectableItemSO GetItem(string itemId)
    {
        if (_itemsDict.ContainsKey(itemId))
        {
            return _itemsDict[itemId].Item;
        }
        return null;
    }
}
