using UnityEngine;

public class GameFlowEventRaiser : MonoBehaviour
{
    [SerializeField] private GameFlowEventChannel _channel;
    [SerializeField] private GameFlowEventSO _event;

    public void Raise()
    {
        _channel?.Raise(_event);
    }
}
