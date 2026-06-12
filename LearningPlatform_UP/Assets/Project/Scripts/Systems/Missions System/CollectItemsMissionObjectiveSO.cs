using UnityEngine;

[CreateAssetMenu(fileName = "CollectItemsObjective", menuName = "Scriptable Objects/Missions/Objectives/Collect Items")]
public class CollectItemsMissionObjectiveSO : MissionObjectiveDefinitionSO
{
    [SerializeField] private CollectableItemSO _item;

    public CollectableItemSO Item => _item;

    public override MissionObjectiveRuntime CreateRuntime(MissionObjectiveContext context)
    {
        return new CollectItemsMissionObjectiveRuntime(this, context.ItemCollectEventChannel);
    }
}

public class CollectItemsMissionObjectiveRuntime : MissionObjectiveRuntime
{
    private readonly CollectItemsMissionObjectiveSO _definition;
    private readonly ItemCollectEventChannel _itemCollectEventChannel;

    public CollectItemsMissionObjectiveRuntime(
        CollectItemsMissionObjectiveSO definition,
        ItemCollectEventChannel itemCollectEventChannel) : base(definition)
    {
        _definition = definition;
        _itemCollectEventChannel = itemCollectEventChannel;
    }

    public override void Start()
    {
        _itemCollectEventChannel?.RegisterListener(OnItemCollected);
    }

    public override void Stop()
    {
        _itemCollectEventChannel?.UnregisterListener(OnItemCollected);
    }

    private void OnItemCollected(ItemCollectEventData data)
    {
        if (_definition.Item != null && data.Item.Id != _definition.Item.Id)
        {
            return;
        }

        AddProgress(data.Amount);
    }
}
