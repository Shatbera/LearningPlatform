using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ResearchBeginEventChannel", menuName = "Scriptable Objects/Event Channels/Research Begin")]

public class ResearchBeginEventChannel : EventChannelBase<ResearchBeginEventData>
{
    
}

public struct ResearchBeginEventData
{
    public ResearchSampleSO ResearchSample;
    public ResearchTask ResearchTask;
}

public class ResearchTask
{
    public float Progress;
    public event Action Complete;

    public void RaiseComplete()
    {
        Complete?.Invoke();
        Complete = null;
    }
}
