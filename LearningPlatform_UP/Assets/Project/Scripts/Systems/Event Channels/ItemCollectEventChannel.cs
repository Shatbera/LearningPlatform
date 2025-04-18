using UnityEngine;

[CreateAssetMenu(fileName = "ItemCollectEventChannel", menuName = "Scriptable Objects/Event Channels/Item Collect Event Channel")]
public class ItemCollectEventChannel : EventChannelBase<ItemCollectEventData>
{
    
}

public struct ItemCollectEventData
{
    public ICollectableItem Item;
    public int Amount;
}
