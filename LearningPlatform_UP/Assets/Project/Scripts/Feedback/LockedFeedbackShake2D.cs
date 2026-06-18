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
    [SerializeField] private float _strength = 0.12f;
    [SerializeField] private int _vibrato = 12;
    [SerializeField] private float _randomness = 0f;

    private Tween _shakeTween;
    private Vector3 _startLocalPosition;
    private Quaternion _startLocalRotation;

    private Transform Target => _target == null ? transform : _target;

    private void OnDestroy()
    {
        _shakeTween?.Kill();
    }

    public void Play()
    {
        Transform target = Target;
        ResetShake();

        _startLocalPosition = target.localPosition;
        _startLocalRotation = target.localRotation;

        _shakeTween = CreateShakeTween(target)
            .OnKill(ResetTarget)
            .OnComplete(ResetTarget);
    }

    private Tween CreateShakeTween(Transform target)
    {
        return _mode switch
        {
            ShakeMode.Vertical => target.DOShakePosition(_duration, new Vector3(0f, _strength, 0f), _vibrato, _randomness, false, true),
            ShakeMode.Rotation => target.DOShakeRotation(_duration, new Vector3(0f, 0f, _strength), _vibrato, _randomness, true),
            _ => target.DOShakePosition(_duration, new Vector3(_strength, 0f, 0f), _vibrato, _randomness, false, true)
        };
    }

    private void ResetShake()
    {
        if (_shakeTween != null && _shakeTween.IsActive())
        {
            _shakeTween.Kill();
        }

        ResetTarget();
    }

    private void ResetTarget()
    {
        Target.localPosition = _startLocalPosition;
        Target.localRotation = _startLocalRotation;
        _shakeTween = null;
    }
}
