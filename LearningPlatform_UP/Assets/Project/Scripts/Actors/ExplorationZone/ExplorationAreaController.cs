using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExplorationAreaController : Singleton<ExplorationAreaController>
{
    [SerializeField] private CinemachineCamera _objectCamera;

    [SerializeField] private Button _exitButton;

    [SerializeField] private Camera _mainCamera;

    [SerializeField] private ScreenFade _screenFade;
    [SerializeField] private SceneLoadingServiceRefSO _sceneLoadingServiceRef;

    private const float ZOOM_DURATION = 0.75f;
    private const float FADE_DURATION = 0.3f;
    private const float FADE_DELAY = 0.1f;

    private const string MENU_SCENE = "Menu";
    private ISceneLoadingService _sceneLoadingService;
    private string _loadedAreaSceneName;
    private bool _explorationAreaLoaded = false;

    protected override void Awake()
    {
        base.Awake();
        if (_exitButton != null)
        {
            _exitButton.onClick.AddListener(Exit);
        }
    }

    private void Start()
    {
        if (!TryGetSceneLoadingService(out _sceneLoadingService))
        {
            return;
        }

        _sceneLoadingService.VisibilityModeChanged += OnVisibilityModeChanged;
        OnVisibilityModeChanged(_sceneLoadingService.CurrentVisibilityMode);
    }

    private void OnDestroy()
    {
        if (_exitButton != null)
        {
            _exitButton.onClick.RemoveListener(Exit);
        }

        if (_sceneLoadingService != null)
        {
            _sceneLoadingService.VisibilityModeChanged -= OnVisibilityModeChanged;
            _sceneLoadingService = null;
        }
    }

    public void LoadArea(string sceneName, bool zoomCamera, Action onComplete = null)
    {
        LoadArea(sceneName, zoomCamera, SceneVisibilityMode.Planet, onComplete);
    }

    public void LoadArea(string sceneName, bool zoomCamera, SceneVisibilityMode visibilityMode, Action onComplete = null)
    {
        StartCoroutine(LoadAreaCoroutine(sceneName, zoomCamera, visibilityMode, onComplete));
    }

    private IEnumerator LoadAreaCoroutine(string sceneName, bool zoomCamera, SceneVisibilityMode visibilityMode, Action onComplete)
    {
        transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);


        if (zoomCamera)
        {
            float startOrtho = _objectCamera.Lens.OrthographicSize;
            bool fading = false;

            for (float t = 0; t < ZOOM_DURATION; t += Time.deltaTime)
            {
                float progress = t / ZOOM_DURATION;
                _objectCamera.Lens.OrthographicSize = Mathf.Lerp(startOrtho, 0.1f, progress);
                if (ZOOM_DURATION - t <= FADE_DURATION && !fading)
                {
                    _screenFade.Fade(true, FADE_DURATION);
                    fading = true;
                }
                yield return null;
            }

            _objectCamera.Lens.OrthographicSize = startOrtho;
        }
        else
        {
            _screenFade.Fade(true, FADE_DURATION);
            yield return new WaitForSeconds(FADE_DURATION);
        }

        bool sceneLoaded = false;
        if (!SwitchScene(true, sceneName, visibilityMode, () => sceneLoaded = true))
        {
            _screenFade.Fade(false, FADE_DURATION);
            yield break;
        }

        _explorationAreaLoaded = true;
        yield return new WaitUntil(() => sceneLoaded);

        yield return new WaitForSeconds(FADE_DELAY);
        _screenFade.Fade(false, FADE_DURATION);

        onComplete?.Invoke();
    }

    public bool SwitchScene(bool exploration, string sceneName = null, SceneVisibilityMode visibilityMode = SceneVisibilityMode.Planet, Action onComplete = null)
    {
        if (_sceneLoadingService == null && !TryGetSceneLoadingService(out _sceneLoadingService))
        {
            return false;
        }

        bool operationStarted;
        if (exploration)
        {
            operationStarted = _sceneLoadingService.TryLoadAdditiveScene(sceneName, visibilityMode, () =>
            {
                _loadedAreaSceneName = sceneName;
                onComplete?.Invoke();
            });
        }
        else
        {
            string areaSceneName = _loadedAreaSceneName;
            if (string.IsNullOrWhiteSpace(areaSceneName))
            {
                Debug.LogWarning("No exploration area scene is loaded.", this);
                return false;
            }

            operationStarted = _sceneLoadingService.TryUnloadScene(areaSceneName, () =>
            {
                if (_loadedAreaSceneName == areaSceneName)
                {
                    _loadedAreaSceneName = null;
                }

                onComplete?.Invoke();
            });
        }

        if (operationStarted)
        {
            SetMainCameraEnabled(!exploration);
        }

        return operationStarted;
    }

    public void Exit()
    {
        if (_explorationAreaLoaded)
        {
            StartCoroutine(ExitCoroutine());
        }
        else
        {
            SceneManager.LoadScene(MENU_SCENE);
        }
    }

    private IEnumerator ExitCoroutine()
    {
        _screenFade.Fade(true, FADE_DURATION);
        yield return new WaitForSeconds(FADE_DURATION);
        bool sceneUnloaded = false;
        bool unloadStarted = SwitchScene(false, onComplete: () => sceneUnloaded = true);
        if (unloadStarted)
        {
            yield return new WaitUntil(() => sceneUnloaded);
        }
        else
        {
            SetMainCameraEnabled(true);
        }

        _explorationAreaLoaded = false;
        yield return new WaitForSeconds(FADE_DELAY);
        _screenFade.Fade(false, FADE_DURATION);
    }

    private void OnVisibilityModeChanged(SceneVisibilityMode visibilityMode)
    {
        SetMainCameraEnabled(visibilityMode == SceneVisibilityMode.Main);

        if (visibilityMode == SceneVisibilityMode.Planet || visibilityMode == SceneVisibilityMode.Lab)
        {
            return;
        }

        _loadedAreaSceneName = null;
        _explorationAreaLoaded = false;
    }

    private void SetMainCameraEnabled(bool enabled)
    {
        if (_mainCamera != null)
        {
            _mainCamera.enabled = enabled;
        }
    }

    private bool TryGetSceneLoadingService(out ISceneLoadingService sceneLoadingService)
    {
        sceneLoadingService = null;
        if (_sceneLoadingServiceRef == null)
        {
            Debug.LogError($"{nameof(ExplorationAreaController)} has no scene loading service ref assigned.", this);
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
