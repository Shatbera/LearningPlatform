using UnityEngine;

public abstract class CollectableItemSO : ScriptableObject, ICollectableItem
{
    public abstract string Id { get; }
    public abstract string DisplayName { get; }
    public abstract Sprite Icon { get; }
}
