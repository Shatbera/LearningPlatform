using System;

public interface ICollectableItem
{
    public CollectableItemSO ItemSO { get; }

    public static event Action<ICollectableItem, int> PickupRequested;
    public static void RequestPickup(ICollectableItem item, int amount)
    {
        PickupRequested?.Invoke(item, amount);
    }

    public void OnPickup();
}
