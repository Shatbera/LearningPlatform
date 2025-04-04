using UnityEngine;

public class ResearchSample : ClickableObject, ICollectableItem
{
    [SerializeField] private ResearchSampleSO _sampleSO;

    public CollectableItemSO ItemSO => _sampleSO;

    public override void OnClick()
    {
        ICollectableItem.RequestPickup(this, 1);
    }

    public void OnPickup()
    {
        Destroy(gameObject);
    }
}
