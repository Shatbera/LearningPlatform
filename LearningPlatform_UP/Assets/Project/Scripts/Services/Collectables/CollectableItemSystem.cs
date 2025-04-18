using System.Collections.Generic;
using UnityEngine;
public class CollectableItemSystem<TItem> : ICollectableItemSystem<TItem>
    where TItem : ICollectableItem
{
    private readonly Dictionary<string, CollectableItemEntry<TItem>> _items = new();

    public CollectableItemSystem(IEnumerable<TItem> items)
    {
        foreach (var item in items)
        {
            _items[item.Id] = new CollectableItemEntry<TItem>(item);
        }
    }

    public bool AddItem(string itemId, int amount)
    {
        if (!_items.TryGetValue(itemId, out var entry))
            return false;

        entry.ChangeAmount(amount);
        Debug.Log($"added {amount} {entry.Item.DisplayName}");
        return true;
    }

    public bool TakeItem(string itemId, int amount)
    {
        if (!_items.TryGetValue(itemId, out var entry))
            return false;

        if (entry.Amount < amount)
            return false;

        entry.ChangeAmount(-amount);
        return true;
    }

    public CollectableItemEntry<TItem> GetItem(string itemId)
    {
        return _items.TryGetValue(itemId, out var entry) ? entry : null;
    }

    public IEnumerable<CollectableItemEntry<TItem>> GetAll()
    {
        return _items.Values;
    }
}
