using System.Collections.Generic;
using UnityEngine;

public class CollectableItemSystem<TItem, TState> : ICollectableItemSystem<TItem, TState>, ISaveable
    where TItem : ICollectableItem
    where TState : CollectableItemState
{
    private readonly Dictionary<string, CollectableItemEntry<TItem, TState>> _items = new();
    private readonly string _saveKey;

    public CollectableItemSystem(IEnumerable<(TItem, TState)> items, string saveKey)
    {
        _saveKey = saveKey;

        foreach (var item in items)
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

    // ------------------------
    // ISaveable implementation
    // ------------------------

    public string SaveKey => _saveKey;

    [System.Serializable]
    public class SaveData
    {
        public List<TState> States = new();
    }

    public object CaptureState()
    {
        var saveData = new SaveData();
        foreach (var entry in _items.Values)
            saveData.States.Add(entry.State);
        return saveData;
    }

    public void RestoreState(object data)
    {
        if (data is not SaveData saveData) return;

        foreach (var state in saveData.States)
        {
            if (_items.TryGetValue(state.Id, out var entry))
            {
                entry.State = state;
            }
        }
    }
}
