using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectableItemSystem<TItem> where TItem : ICollectableItem
{
    public bool AddItem(string itemId, int amount);
    public bool TakeItem(string itemId, int amount);
    CollectableItemEntry<TItem> GetItem(string itemId);
    public IEnumerable<CollectableItemEntry<TItem>> GetAll();
}

public interface ICollectableItem
{
    public abstract string Id { get; }
    public abstract string DisplayName { get; }
    public abstract Sprite Icon { get; }
}
public class CollectableItemEntry<TItem> where TItem : ICollectableItem
{
    public TItem Item;
    public int Amount;
    public event Action<int> Updated;

    public CollectableItemEntry(TItem item, int initialAmount = 0)
    {
        Item = item;
        Amount = initialAmount;
    }
    public void ChangeAmount(int delta)
    {
        Amount += delta;
        Updated?.Invoke(delta);
    }
}
