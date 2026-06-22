using UnityEngine;
using UnityEngine.Events;

public class GrowPlantWaterPot : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _pickRadius = 1.2f;
    [SerializeField] private float _wateringDistance = 1.25f;
    [SerializeField] private bool _returnAfterWatering = true;
    [SerializeField] private GameObject _waterFlow;
    [SerializeField] private GameObject _frame;
    [SerializeField] private float _pourRotationDegrees = -18f;
    [SerializeField] private float _pourRotationSpeed = 180f;
    [SerializeField] private UnityEvent _watered;

    private Transform _wateringTarget;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Quaternion _pourRotation;
    private Vector3 _dragOffset;
    private bool _isDragging;
    private bool _frameStartActive;

    public bool IsWatered { get; private set; }

    private void Awake()
    {
        _startPosition = transform.position;
        _startRotation = transform.localRotation;
        _pourRotation = _startRotation * Quaternion.Euler(0f, 0f, _pourRotationDegrees);

        _frameStartActive = _frame != null && _frame.activeSelf;
        SetWaterFlowActive(false);

        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    private void Update()
    {
        UpdatePourVisuals();

        if (IsWatered)
        {
            return;
        }

        GrowPlantPointerState pointer = GrowPlantPointerInput.Get(_camera);

        if (pointer.Down && IsNear(transform.position, pointer.WorldPosition, _pickRadius))
        {
            _isDragging = true;
            _dragOffset = transform.position - pointer.WorldPosition;
            SetWaterFlowActive(false);
            SetFrameActive(false);
        }

        if (_isDragging && pointer.Held)
        {
            DragTo(pointer.WorldPosition);
        }

        if (_isDragging && pointer.Up)
        {
            TryWaterPlant();
            _isDragging = false;
            SetWaterFlowActive(false);
            SetFrameActive(_frameStartActive);
        }
    }

    public void SetCamera(Camera targetCamera)
    {
        _camera = targetCamera;
    }

    public void SetWateringTarget(Transform target)
    {
        _wateringTarget = target;
    }

    private void DragTo(Vector3 pointerWorldPosition)
    {
        Vector3 targetPosition = pointerWorldPosition + _dragOffset;
        targetPosition.z = transform.position.z;
        transform.position = targetPosition;
    }

    private void TryWaterPlant()
    {
        bool wateredPlant = _wateringTarget != null && IsNear(_wateringTarget.position, transform.position, _wateringDistance);

        if (_returnAfterWatering)
        {
            transform.position = _startPosition;
        }

        if (!wateredPlant)
        {
            return;
        }

        IsWatered = true;
        _watered?.Invoke();
    }

    private void UpdatePourVisuals()
    {
        Quaternion targetRotation = _isDragging ? _pourRotation : _startRotation;
        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation,
            targetRotation,
            _pourRotationSpeed * Time.deltaTime);

        if (_isDragging && Quaternion.Angle(transform.localRotation, _pourRotation) <= Mathf.Epsilon)
        {
            SetWaterFlowActive(true);
        }
    }

    private void SetWaterFlowActive(bool active)
    {
        if (_waterFlow == null || _waterFlow.activeSelf == active)
        {
            return;
        }

        _waterFlow.SetActive(active);
    }

    private void SetFrameActive(bool active)
    {
        if (_frame == null || _frame.activeSelf == active)
        {
            return;
        }

        _frame.SetActive(active);
    }

    private bool IsNear(Vector3 firstPosition, Vector3 secondPosition, float radius)
    {
        return Vector2.Distance(firstPosition, secondPosition) <= radius;
    }
}
