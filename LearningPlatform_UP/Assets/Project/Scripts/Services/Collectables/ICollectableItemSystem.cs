using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectableItemSystem<TItem, TState> where TItem : ICollectableItem where TState : CollectableItemState
{
    public bool AddItem(string itemId, int amount);
    public bool TakeItem(string itemId, int amount);
    CollectableItemEntry<TItem, TState> GetItem(string itemId);
    public IEnumerable<CollectableItemEntry<TItem, TState>> GetAll();
}

public interface ICollectableItem
{
    public abstract string Id { get; }
    public abstract string DisplayName { get; }
    public abstract Sprite Icon { get; }
}

[System.Serializable]
public abstract class CollectableItemState
{
    public string Id;
    public int Amount;
    public event Action<int> AmountChanged;
    public CollectableItemState(string id, int initialAmount = 0)
    {
        Id = id;
        Amount = initialAmount;
    }
    public void ChangeAmount(int delta)
    {
        Amount += delta;
        AmountChanged?.Invoke(delta);
    }
}
public sealed class CollectableItemEntry<TItem, TState> where TItem : ICollectableItem where TState : CollectableItemState
{
    public TItem Item;
    public TState State;
    
    public CollectableItemEntry(TItem item, TState state)
    {
        Item = item;
        State = state;
    }

    public void ChangeAmount(int delta) => State.ChangeAmount(delta);
}
