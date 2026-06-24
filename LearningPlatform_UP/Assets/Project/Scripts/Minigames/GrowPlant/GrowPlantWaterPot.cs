using UnityEngine;
using UnityEngine.Serialization;

public class GrowPlantWaterPot : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _pickRadius = 1.2f;
    [SerializeField, FormerlySerializedAs("_returnAfterWatering")] private bool _returnAfterRelease = true;
    [SerializeField] private GameObject _waterFlow;
    [SerializeField] private GameObject _frame;
    [SerializeField] private float _pourRotationDegrees = -18f;
    [SerializeField] private float _pourRotationSpeed = 180f;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Quaternion _pourRotation;
    private Vector3 _dragOffset;
    private bool _isDragging;
    private bool _canDrag = true;
    private bool _frameStartActive;

    private void Awake()
    {
        _startPosition = transform.position;
        _startRotation = transform.localRotation;
        _pourRotation = _startRotation * Quaternion.Euler(0f, 0f, _pourRotationDegrees);

        _frameStartActive = _frame != null && _frame.activeSelf;
        SetWaterFlowActive(false);
    }

    private void Update()
    {
        UpdatePourVisuals();

        if (!_canDrag)
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
            _isDragging = false;
            SetWaterFlowActive(false);
            SetFrameActive(_frameStartActive);

            if (_returnAfterRelease)
            {
                transform.position = _startPosition;
            }
        }
    }

    public void SetCamera(Camera targetCamera)
    {
        _camera = targetCamera;
    }

    public void SetCanDrag(bool canDrag)
    {
        _canDrag = canDrag;

        if (_canDrag)
        {
            return;
        }

        _isDragging = false;
        SetWaterFlowActive(false);
        SetFrameActive(_frameStartActive);

        if (_returnAfterRelease)
        {
            transform.position = _startPosition;
        }
    }

    private void DragTo(Vector3 pointerWorldPosition)
    {
        Vector3 targetPosition = pointerWorldPosition + _dragOffset;
        targetPosition.z = transform.position.z;
        transform.position = targetPosition;
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
