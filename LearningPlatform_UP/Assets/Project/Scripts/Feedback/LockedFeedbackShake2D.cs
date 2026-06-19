using DG.Tweening;
using UnityEngine;

public class LockedFeedbackShake2D : MonoBehaviour
{
    private enum ShakeMode
    {
        Horizontal,
        Vertical,
        Rotation
    }

    [SerializeField] private Transform _target;
    [SerializeField] private ShakeMode _mode = ShakeMode.Horizontal;
    [SerializeField] private float _duration = 0.35f;
    [SerializeField] private float _positionStrength = 0.12f;
    [SerializeField] private float _rotationStrength = 12f;
    [SerializeField] private int _vibrato = 12;
    [SerializeField] private float _elasticity = 0.8f;

    private Tween _feedbackTween;
    private Transform _activeTarget;
    private Vector3 _startLocalPosition;
    private Quaternion _startLocalRotation;

    private Transform Target => _target == null ? transform : _target;

    private void OnDestroy()
    {
        _feedbackTween?.Kill();
    }

    public void Play()
    {
        Transform target = Target;
        StopActiveTween();
        RestoreActiveTarget();

        _activeTarget = target;
        _startLocalPosition = target.localPosition;
        _startLocalRotation = target.localRotation;

        _feedbackTween = CreateTween(target).OnComplete(RestoreActiveTarget);
    }

    private Tween CreateTween(Transform target)
    {
        return _mode switch
        {
            ShakeMode.Vertical => target.DOPunchPosition(Vector3.up * _positionStrength, _duration, _vibrato, _elasticity),
            ShakeMode.Rotation => target.DOPunchRotation(Vector3.forward * _rotationStrength, _duration, _vibrato, _elasticity),
            _ => target.DOPunchPosition(Vector3.right * _positionStrength, _duration, _vibrato, _elasticity)
        };
    }

    private void StopActiveTween()
    {
        if (_feedbackTween != null && _feedbackTween.IsActive())
        {
            _feedbackTween.Kill();
        }

        _feedbackTween = null;
    }

    private void RestoreActiveTarget()
    {
        if (_activeTarget != null)
        {
            _activeTarget.localPosition = _startLocalPosition;
            _activeTarget.localRotation = _startLocalRotation;
        }

        _feedbackTween = null;
        _activeTarget = null;
    }
}
