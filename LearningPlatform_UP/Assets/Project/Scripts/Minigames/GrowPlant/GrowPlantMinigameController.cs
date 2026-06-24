using UnityEngine;
using UnityEngine.Events;

public class GrowPlantMinigameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GrowPlantWaterPot _waterPot;
    [SerializeField] private GrowPlantLightSource _lightSource;
    [SerializeField] private GrowPlantView _plant;
    [SerializeField] private GrowPlantWaterReceiver _waterReceiver;
    [SerializeField] private Camera _camera;

    [SerializeField] private UnityEvent _completed;

    private bool _completedRaised;

    private void Awake()
    {
        if (_waterPot != null)
        {
            _waterPot.SetCamera(_camera);
        }

        if (_lightSource != null)
        {
            _lightSource.SetCamera(_camera);
            _lightSource.SetAimTarget(_plant == null ? null : _plant.Target);
        }
    }

    private void Update()
    {
        if (_completedRaised || _waterPot == null || _lightSource == null || _plant == null)
        {
            return;
        }

        TryComplete();
    }

    private void TryComplete()
    {
        if (_waterReceiver != null && _waterReceiver.IsComplete)
        {
            _waterPot.SetCanDrag(false);
        }

        if (_lightSource != null && _lightSource.IsFullyCharged)
        {
            _lightSource.SetCanRotate(false);
        }

        if (_waterReceiver == null || !_waterReceiver.IsComplete || !_lightSource.IsFullyCharged)
        {
            return;
        }

        _completedRaised = true;
        _plant.Grow();
        _lightSource.SetCanRotate(false);
        _completed?.Invoke();
    }
}
