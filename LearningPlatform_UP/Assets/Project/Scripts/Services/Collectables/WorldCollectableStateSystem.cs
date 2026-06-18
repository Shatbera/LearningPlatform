using System.Collections.Generic;

public class WorldCollectableStateSystem : IWorldCollectableStateSystem, ISaveable
{
    private readonly HashSet<string> _collectedIds = new();

    public static IWorldCollectableStateSystem Current { get; private set; }

    public WorldCollectableStateSystem()
    {
        Current = this;
    }

    public string SaveKey => "worldCollectableStateSystem";

    public bool IsCollected(string instanceId)
    {
        return !string.IsNullOrEmpty(instanceId) && _collectedIds.Contains(instanceId);
    }

    public void MarkCollected(string instanceId)
    {
        if (!string.IsNullOrEmpty(instanceId))
        {
            _collectedIds.Add(instanceId);
        }
    }

    public IEnumerable<string> GetCollectedIds()
    {
        return _collectedIds;
    }

    public object CaptureState()
    {
        var saveData = new WorldCollectableStateSystemSaveData();
        saveData.CollectedIds.AddRange(_collectedIds);
        return saveData;
    }

    public void RestoreState(object data)
    {
        if (data is not WorldCollectableStateSystemSaveData saveData)
        {
            return;
        }

        _collectedIds.Clear();
        foreach (string collectedId in saveData.CollectedIds)
        {
            if (!string.IsNullOrEmpty(collectedId))
            {
                _collectedIds.Add(collectedId);
            }
        }
    }
}

[System.Serializable]
public class WorldCollectableStateSystemSaveData
{
    public List<string> CollectedIds = new();
}
