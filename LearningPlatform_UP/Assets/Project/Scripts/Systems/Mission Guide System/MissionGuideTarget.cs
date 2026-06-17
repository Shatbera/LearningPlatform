using UnityEngine;

public class MissionGuideTarget : MonoBehaviour
{
    [SerializeField] private MissionGuideTargetSO _definition;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Renderer _visibilityRenderer;

    public MissionGuideTargetSO Definition => _definition;
    public Transform TargetTransform => _targetTransform == null ? transform : _targetTransform;
    public Sprite Icon => _definition == null ? null : _definition.Icon;

    private void Awake()
    {
        if (_visibilityRenderer == null)
        {
            _visibilityRenderer = GetComponentInChildren<Renderer>();
        }
    }

    public bool TryGetVisibilityBounds(out Bounds bounds)
    {
        if (_visibilityRenderer == null)
        {
            bounds = default;
            return false;
        }

        bounds = _visibilityRenderer.bounds;
        return true;
    }
}
