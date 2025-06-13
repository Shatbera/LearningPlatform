using UnityEngine;
using UnityEngine.Localization;

public abstract class CollectableItemSO : ScriptableObject, ICollectableItem
{
    public abstract string Id { get; }
    public abstract LocalizedString LocalizedDisplayName { get; }
    public abstract Sprite Icon { get; }
}
