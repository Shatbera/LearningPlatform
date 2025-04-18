using System;

public interface ICollectableItem
{
    public CollectableItemSO ItemSO { get; }

    public void OnPickup();
}
