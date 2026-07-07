using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UiSceneVisibility : MonoBehaviour
{
    [SerializeField] private SceneLoadingServiceRefSO _sceneLoadingServiceRef;
    [SerializeField] private SceneVisibilityMode[] _visibleInModes = { SceneVisibilityMode.Main };

    private ISceneLoadingService _sceneLoadingService;
    private CanvasGroup _canvasGroup;
    private bool _started;
    private bool _subscribed;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        if (_started)
        {
            Subscribe();
        }
    }

    private void Start()
    {
        _started = true;
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (_subscribed)
        {
            return;
        }

        if (!TryGetSceneLoadingService(out _sceneLoadingService))
        {
            return;
        }

        _sceneLoadingService.VisibilityModeChanged += OnVisibilityModeChanged;
        _subscribed = true;
        ApplyVisibility(_sceneLoadingService.CurrentVisibilityMode);
    }

    private void Unsubscribe()
    {
        if (!_subscribed || _sceneLoadingService == null)
        {
            _sceneLoadingService = null;
            _subscribed = false;
            return;
        }

        _sceneLoadingService.VisibilityModeChanged -= OnVisibilityModeChanged;
        _sceneLoadingService = null;
        _subscribed = false;
    }

    private void OnVisibilityModeChanged(SceneVisibilityMode mode)
    {
        ApplyVisibility(mode);
    }

    private void ApplyVisibility(SceneVisibilityMode mode)
    {
        SetVisible(IsVisibleInMode(mode));
    }

    private bool IsVisibleInMode(SceneVisibilityMode mode)
    {
        if (_visibleInModes == null)
        {
            return false;
        }

        foreach (SceneVisibilityMode visibleMode in _visibleInModes)
        {
            if (visibleMode == mode)
            {
                return true;
            }
        }

        return false;
    }

    private void SetVisible(bool visible)
    {
        if (_canvasGroup == null)
        {
            Debug.LogError($"{nameof(UiSceneVisibility)} has no CanvasGroup.", this);
            return;
        }

        _canvasGroup.alpha = visible ? 1f : 0f;
        _canvasGroup.interactable = visible;
        _canvasGroup.blocksRaycasts = visible;
    }

    private bool TryGetSceneLoadingService(out ISceneLoadingService sceneLoadingService)
    {
        sceneLoadingService = null;
        if (_sceneLoadingServiceRef == null)
        {
            Debug.LogError($"{nameof(UiSceneVisibility)} has no scene loading service ref assigned.", this);
            return false;
        }

        sceneLoadingService = _sceneLoadingServiceRef.Service;
        if (sceneLoadingService == null)
        {
            Debug.LogError("Scene loading service is not installed.", this);
            return false;
        }

        return true;
    }
}
