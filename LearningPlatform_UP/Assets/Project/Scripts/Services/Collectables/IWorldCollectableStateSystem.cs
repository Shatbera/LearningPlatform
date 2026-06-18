using System.Collections.Generic;

public interface IWorldCollectableStateSystem
{
    bool IsCollected(string instanceId);
    void MarkCollected(string instanceId);
    IEnumerable<string> GetCollectedIds();
}
