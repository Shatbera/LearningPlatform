using System;
using UnityEngine;

public abstract class EventChannelBase<T> : ScriptableObject
{
    private event Action<T> listeners;

    public void Raise(T value) => listeners?.Invoke(value);
    public void RegisterListener(Action<T> listener) => listeners += listener;
    public void UnregisterListener(Action<T> listener) => listeners -= listener;
}
