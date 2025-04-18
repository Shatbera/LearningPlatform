using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EventChannel", menuName = "Scriptable Objects/Event Channels/Event Channel")]
public class EventChannel : ScriptableObject
{
    private event Action listeners;

    public void Raise()
    {
        listeners?.Invoke();
    }

    public void RegisterListener(Action listener)
    {
        listeners += listener;
    }

    public void UnregisterListener(Action listener)
    {
        listeners -= listener;
    }
}
