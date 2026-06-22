using UnityEngine;

public class GrowPlantLightSource : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _aimDirectionTransform;
    [SerializeField] private float _pickRadius = 1.25f;
    [SerializeField] private float _aimToleranceDegrees = 18f;
    [SerializeField] private float _aimAxisOffsetDegrees;

    private Transform _aimTarget;
    private bool _isRotating;
    private bool _canRotate = true;
    private float _dragAngleOffsetDegrees;

    public bool IsAimedAtTarget => IsAimedAt(_aimTarget);

    private void Awake()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
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
    }

    public void SetCamera(Camera targetCamera)
    {
        _camera = targetCamera;
    }

    public void SetAimTarget(Transform target)
    {
        _aimTarget = target;
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
