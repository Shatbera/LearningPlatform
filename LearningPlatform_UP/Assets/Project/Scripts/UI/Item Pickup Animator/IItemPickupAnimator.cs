using UnityEngine;

public interface IPickableItem
{
    public SpriteRenderer Renderer { get; }
}
public interface IItemPickupAnimator
{
    public void AnimatePickup(IPickableItem item, System.Action onComplete = null);
}
