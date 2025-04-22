using System.Collections.Generic;
using UnityEngine;
public class CollectableItemSystem<TItem, TState> : ICollectableItemSystem<TItem, TState>
    where TItem : ICollectableItem where TState : ResearchSampleItemState
{
    private readonly Dictionary<string, CollectableItemEntry<TItem, TState>> _items = new();

    public CollectableItemSystem(IEnumerable<(TItem, TState)> items)
    {
        foreach ((TItem, TState) item in items)
        {
            _items[item.Item1.Id] = new CollectableItemEntry<TItem, TState>(item.Item1, item.Item2);
        }
    }

    public bool AddItem(string itemId, int amount)
    {
        if (!_items.TryGetValue(itemId, out var entry))
            return false;

        entry.State.ChangeAmount(amount);
        Debug.Log($"added {amount} {entry.Item.DisplayName}");
        return true;
    }

    public bool TakeItem(string itemId, int amount)
    {
        if (!_items.TryGetValue(itemId, out var entry))
            return false;

        if (entry.State.Amount < amount)
            return false;

        entry.State.ChangeAmount(-amount);
        return true;
    }

    public CollectableItemEntry<TItem, TState> GetItem(string itemId)
    {
        return _items.TryGetValue(itemId, out var entry) ? entry : null;
    }

    public IEnumerable<CollectableItemEntry<TItem, TState>> GetAll()
    {
        return _items.Values;
    }
}
