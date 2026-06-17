using UnityEngine;
using UnityEngine.UI;

public class MissionGuideArrowUI : MonoBehaviour
{
    [SerializeField] private MissionGuideSystemRefSO _missionGuideSystemRef;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Camera _worldCamera;
    [SerializeField] private RectTransform _container;
    [SerializeField] private RectTransform _arrow;
    [SerializeField] private Image _iconImage;
    [SerializeField] private float _edgePadding = 120f;
    [SerializeField] private bool _hideWhenTargetOnScreen = true;
    [SerializeField] private float _screenVisibilityPadding = 80f;

    private IMissionGuideSystem _missionGuideSystem;
    private MissionGuideTarget _currentTarget;
    private RectTransform _canvasRect;

    private void Awake()
    {
        EnsureReferences();
        SetVisible(false);
    }

    private void OnEnable()
    {
        Bind();
        Refresh(_missionGuideSystem?.CurrentTarget);
    }

    private void Start()
    {
        Bind();
        Refresh(_missionGuideSystem?.CurrentTarget);
    }

    private void OnDisable()
    {
        if (_missionGuideSystem != null)
        {
            _missionGuideSystem.CurrentTargetChanged -= Refresh;
            _missionGuideSystem = null;
        }
    }

    private void LateUpdate()
    {
        if (_missionGuideSystem == null)
        {
            Bind();
        }

        UpdateArrow();
    }

    private void Bind()
    {
        if (_missionGuideSystem != null || _missionGuideSystemRef == null || _missionGuideSystemRef.Service == null)
        {
            return;
        }

        _missionGuideSystem = _missionGuideSystemRef.Service;
        _missionGuideSystem.CurrentTargetChanged += Refresh;
        Refresh(_missionGuideSystem.CurrentTarget);
    }

    private void Refresh(MissionGuideTarget target)
    {
        _currentTarget = target;

        if (_iconImage != null)
        {
            Sprite icon = _currentTarget == null ? null : _currentTarget.Icon;
            _iconImage.sprite = icon;
            _iconImage.enabled = icon != null;
        }

        UpdateArrow();
    }

    private void UpdateArrow()
    {
        EnsureReferences();

        if (_currentTarget == null || _currentTarget.TargetTransform == null || _canvasRect == null)
        {
            SetVisible(false);
            return;
        }

        Camera worldCamera = _worldCamera == null ? Camera.main : _worldCamera;
        if (worldCamera == null)
        {
            SetVisible(false);
            return;
        }

        Vector2 screenPoint = worldCamera.WorldToScreenPoint(_currentTarget.TargetTransform.position);
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 direction = screenPoint - screenCenter;

        if (direction.sqrMagnitude <= 0.001f)
        {
            SetVisible(false);
            return;
        }

        if (_hideWhenTargetOnScreen && IsTargetVisible(worldCamera, _currentTarget, screenPoint))
        {
            SetVisible(false);
            return;
        }

        Vector2 clampedScreenPoint = ClampToScreenEdge(screenCenter, direction.normalized);
        Camera uiCamera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, clampedScreenPoint, uiCamera, out Vector2 localPoint))
        {
            SetVisible(false);
            return;
        }

        _container.anchoredPosition = localPoint;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        if (_arrow != null)
        {
            _arrow.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        SetVisible(true);
    }

    private bool IsTargetVisible(Camera worldCamera, MissionGuideTarget target, Vector2 screenPoint)
    {
        if (target.TryGetVisibilityBounds(out Bounds bounds))
        {
            return IsBoundsOnScreen(worldCamera, bounds);
        }

        return IsScreenPointOnScreen(screenPoint);
    }

    private bool IsBoundsOnScreen(Camera worldCamera, Bounds bounds)
    {
        float z = bounds.center.z;
        Vector2 min = worldCamera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.min.y, z));
        Vector2 max = min;

        EncapsulateScreenPoint(worldCamera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, z)), ref min, ref max);
        EncapsulateScreenPoint(worldCamera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, z)), ref min, ref max);
        EncapsulateScreenPoint(worldCamera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.max.y, z)), ref min, ref max);

        return max.x >= _screenVisibilityPadding
            && min.x <= Screen.width - _screenVisibilityPadding
            && max.y >= _screenVisibilityPadding
            && min.y <= Screen.height - _screenVisibilityPadding;
    }

    private void EncapsulateScreenPoint(Vector2 point, ref Vector2 min, ref Vector2 max)
    {
        min = Vector2.Min(min, point);
        max = Vector2.Max(max, point);
    }

    private bool IsScreenPointOnScreen(Vector2 screenPoint)
    {
        return screenPoint.x >= _screenVisibilityPadding
            && screenPoint.x <= Screen.width - _screenVisibilityPadding
            && screenPoint.y >= _screenVisibilityPadding
            && screenPoint.y <= Screen.height - _screenVisibilityPadding;
    }

    private Vector2 ClampToScreenEdge(Vector2 screenCenter, Vector2 direction)
    {
        float halfWidth = Mathf.Max(0f, Screen.width * 0.5f - _edgePadding);
        float halfHeight = Mathf.Max(0f, Screen.height * 0.5f - _edgePadding);
        float scaleX = Mathf.Abs(direction.x) < 0.001f ? float.PositiveInfinity : halfWidth / Mathf.Abs(direction.x);
        float scaleY = Mathf.Abs(direction.y) < 0.001f ? float.PositiveInfinity : halfHeight / Mathf.Abs(direction.y);
        return screenCenter + direction * Mathf.Min(scaleX, scaleY);
    }

    private void EnsureReferences()
    {
        if (_canvas == null)
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        if (_canvas != null)
        {
            _canvasRect = _canvas.transform as RectTransform;
        }
    }

    private void SetVisible(bool visible)
    {
        if (_container != null && _container.gameObject.activeSelf != visible)
        {
            _container.gameObject.SetActive(visible);
        }
    }
}
