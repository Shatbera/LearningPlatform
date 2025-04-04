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

    private void OnEnable()
    {
        ICollectableItem.PickupRequested += RequestItemPickup;
    }


    private void OnDisable()
    {
        ICollectableItem.PickupRequested -= RequestItemPickup;
    }

    private void RequestItemPickup(ICollectableItem item, int amount)
    {
        if(AddItem(item.ItemSO, amount))
        {
            item.OnPickup();
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
