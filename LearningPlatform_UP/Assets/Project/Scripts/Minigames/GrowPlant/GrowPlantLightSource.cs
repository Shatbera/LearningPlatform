using UnityEngine;

public class GrowPlantLightSource : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _aimDirectionTransform;
    [SerializeField] private PlantProgressBar _progressBar;
    [SerializeField] private float _pickRadius = 1.25f;
    [SerializeField] private float _aimToleranceDegrees = 18f;
    [SerializeField] private float _aimAxisOffsetDegrees;
    [SerializeField] private float _progressFillRate = 0.35f;

    private Transform _aimTarget;
    private bool _isRotating;
    private bool _canRotate = true;
    private float _dragAngleOffsetDegrees;
    private float _progress;

    public bool IsAimedAtTarget => IsAimedAt(_aimTarget);
    public bool IsFullyCharged => _progress >= 1f;

    private void Awake()
    {
        UpdateProgressBar();
    }

    private void Update()
    {
        if (!_canRotate)
        {
            return;
        }

        GrowPlantPointerState pointer = GrowPlantPointerInput.Get(_camera);

        if (pointer.Down && IsNear(pointer.WorldPosition))
        {
            _isRotating = true;
            _dragAngleOffsetDegrees = transform.eulerAngles.z - GetPointerAngle(pointer.WorldPosition);
        }

        if (_isRotating && pointer.Held)
        {
            RotateWithPointer(pointer.WorldPosition);
        }

        if (pointer.Up)
        {
            _isRotating = false;
        }

        UpdateSunProgress();
    }

    public void SetCamera(Camera targetCamera)
    {
        _camera = targetCamera;
    }

    public void SetAimTarget(Transform target)
    {
        _aimTarget = target;
    }

    public void SetProgressBar(PlantProgressBar progressBar)
    {
        _progressBar = progressBar;
        UpdateProgressBar();
    }

    public void SetCanRotate(bool canRotate)
    {
        _canRotate = canRotate;

        if (!_canRotate)
        {
            _isRotating = false;
        }
    }

    private void RotateWithPointer(Vector3 pointerWorldPosition)
    {
        float angle = GetPointerAngle(pointerWorldPosition);
        transform.rotation = Quaternion.Euler(0f, 0f, angle + _dragAngleOffsetDegrees);
    }

    private void UpdateSunProgress()
    {
        if (IsFullyCharged || !IsAimedAtTarget)
        {
            return;
        }

        _progress = Mathf.Clamp01(_progress + _progressFillRate * Time.deltaTime);
        UpdateProgressBar();

        if (IsFullyCharged)
        {
            SetCanRotate(false);
        }
    }

    private void UpdateProgressBar()
    {
        if (_progressBar != null)
        {
            _progressBar.SetProgress01(_progress);
        }
    }

    private bool IsAimedAt(Transform target)
    {
        if (target == null)
        {
            return false;
        }

        Transform aimTransform = _aimDirectionTransform == null ? transform : _aimDirectionTransform;
        Vector2 aimDirection = Quaternion.Euler(0f, 0f, _aimAxisOffsetDegrees) * aimTransform.right;
        Vector2 targetDirection = target.position - aimTransform.position;

        if (aimDirection.sqrMagnitude <= Mathf.Epsilon || targetDirection.sqrMagnitude <= Mathf.Epsilon)
        {
            return false;
        }

        return Vector2.Angle(aimDirection, targetDirection) <= _aimToleranceDegrees;
    }

    private bool IsNear(Vector3 pointerWorldPosition)
    {
        return Vector2.Distance(transform.position, pointerWorldPosition) <= _pickRadius;
    }

    private float GetPointerAngle(Vector3 pointerWorldPosition)
    {
        Vector2 direction = pointerWorldPosition - transform.position;

        if (direction.sqrMagnitude <= Mathf.Epsilon)
        {
            return transform.eulerAngles.z;
        }

        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
