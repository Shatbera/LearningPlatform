using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameFlowEventChannel", menuName = "Scriptable Objects/Event Channels/Game Flow Event Channel")]
public class GameFlowEventChannel : ScriptableObject
{
    private event Action<GameFlowEvent> listeners;

    public void Raise(GameFlowEventSO flowEvent)
    {
        if (flowEvent == null)
        {
            return;
        }

        listeners?.Invoke(new GameFlowEvent(flowEvent));
    }

    public void RegisterListener(Action<GameFlowEvent> listener)
    {
        listeners += listener;
    }

    public void UnregisterListener(Action<GameFlowEvent> listener)
    {
        listeners -= listener;
    }
}

public readonly struct GameFlowEvent
{
    public GameFlowEvent(GameFlowEventSO definition)
    {
        Definition = definition;
    }

    public GameFlowEventSO Definition { get; }
    public string Id => Definition == null ? string.Empty : Definition.Id;
}
