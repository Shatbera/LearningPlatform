using UnityEngine;
using UnityEngine.Events;

public class GrowPlantMinigameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GrowPlantWaterPot _waterPot;
    [SerializeField] private GrowPlantLightSource _lightSource;
    [SerializeField] private GrowPlantView _plant;
    [SerializeField] private Camera _camera;

    [SerializeField] private UnityEvent _completed;

    private bool _completedRaised;

    private void Awake()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }

        if (_waterPot != null)
        {
            _waterPot.SetCamera(_camera);
            _waterPot.SetWateringTarget(_plant == null ? null : _plant.Target);
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
        if (!_waterPot.IsWatered || !_lightSource.IsAimedAtTarget)
        {
            return;
        }

        _completedRaised = true;
        _plant.Grow();
        _lightSource.SetCanRotate(false);
        _completed?.Invoke();
    }
}
