using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingService : ISceneLoadingService
{
    public event Action<string> MiniGameLoaded;
    public event Action<string> MiniGameUnloaded;
    public event Action<SceneVisibilityMode> VisibilityModeChanged;

    private string _activeMiniGameSceneName;
    private string _activeAdditiveSceneName;

    public bool IsBusy { get; private set; }
    public string ActiveMiniGameSceneName => _activeMiniGameSceneName;
    public SceneVisibilityMode CurrentVisibilityMode { get; private set; } = SceneVisibilityMode.Main;

    public bool HasActiveMiniGame =>
        !string.IsNullOrWhiteSpace(_activeMiniGameSceneName) &&
        IsSceneLoaded(_activeMiniGameSceneName);

    private bool HasActiveAdditiveScene =>
        !string.IsNullOrWhiteSpace(_activeAdditiveSceneName) &&
        IsSceneLoaded(_activeAdditiveSceneName);

    public bool IsSceneLoaded(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            return false;
        }

        return SceneManager.GetSceneByName(sceneName).isLoaded;
    }

    public bool TryLoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, Action onComplete = null)
    {
        if (!CanUseSceneName(sceneName))
        {
            return false;
        }

        if (mode == LoadSceneMode.Additive && IsSceneLoaded(sceneName))
        {
            Debug.LogWarning($"Scene {sceneName} is already loaded.");
            return false;
        }

        return TryStartOperation(
            () => SceneManager.LoadSceneAsync(sceneName, mode),
            $"load scene {sceneName}",
            () =>
            {
                if (mode == LoadSceneMode.Single)
                {
                    string unloadedMiniGameSceneName = _activeMiniGameSceneName;
                    _activeMiniGameSceneName = null;
                    ClearActiveAdditiveScene();
                    SetVisibilityMode(SceneVisibilityMode.Main);

                    if (!string.IsNullOrWhiteSpace(unloadedMiniGameSceneName))
                    {
                        MiniGameUnloaded?.Invoke(unloadedMiniGameSceneName);
                    }
                }

                onComplete?.Invoke();
            });
    }

    public bool TryLoadAdditiveScene(string sceneName, Action onComplete = null)
    {
        return TryLoadScene(sceneName, LoadSceneMode.Additive, onComplete);
    }

    public bool TryLoadAdditiveScene(string sceneName, SceneVisibilityMode visibilityMode, Action onComplete = null)
    {
        if (!CanUseSceneName(sceneName))
        {
            return false;
        }

        if (IsSceneLoaded(sceneName))
        {
            Debug.LogWarning($"Scene {sceneName} is already loaded.");
            return false;
        }

        if (HasActiveAdditiveScene)
        {
            string sceneToUnload = _activeAdditiveSceneName;
            return TryUnloadScene(sceneToUnload, () =>
            {
                TryLoadTrackedAdditiveScene(sceneName, visibilityMode, onComplete);
            }, updateVisibilityMode: false);
        }

        ClearStaleActiveAdditiveScene();
        return TryLoadTrackedAdditiveScene(sceneName, visibilityMode, onComplete);
    }

    public bool TryUnloadScene(string sceneName, Action onComplete = null)
    {
        return TryUnloadScene(sceneName, onComplete, updateVisibilityMode: true);
    }

    private bool TryUnloadScene(string sceneName, Action onComplete, bool updateVisibilityMode)
    {
        if (!CanUseSceneName(sceneName))
        {
            return false;
        }

        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.IsValid() || !scene.isLoaded)
        {
            Debug.LogWarning($"Scene {sceneName} is not loaded.");
            return false;
        }

        if (SceneManager.sceneCount <= 1)
        {
            Debug.LogWarning($"Cannot unload {sceneName} because it is the only loaded scene.");
            return false;
        }

        return TryStartOperation(
            () => SceneManager.UnloadSceneAsync(sceneName),
            $"unload scene {sceneName}",
            () =>
            {
                bool unloadedActiveAdditiveScene = _activeAdditiveSceneName == sceneName;
                bool unloadedActiveMiniGame = _activeMiniGameSceneName == sceneName;
                if (_activeMiniGameSceneName == sceneName)
                {
                    _activeMiniGameSceneName = null;
                }

                if (unloadedActiveAdditiveScene)
                {
                    ClearActiveAdditiveScene();
                    if (updateVisibilityMode)
                    {
                        SetVisibilityMode(SceneVisibilityMode.Main);
                    }
                }

                if (unloadedActiveMiniGame)
                {
                    MiniGameUnloaded?.Invoke(sceneName);
                }

                onComplete?.Invoke();
            });
    }

    public bool TryLoadMiniGame(string sceneName, Action onComplete = null)
    {
        if (HasActiveMiniGame)
        {
            Debug.LogWarning($"Mini game scene {_activeMiniGameSceneName} is already loaded.");
            return false;
        }

        return TryLoadAdditiveScene(sceneName, SceneVisibilityMode.MiniGame, () =>
        {
            _activeMiniGameSceneName = sceneName;
            MiniGameLoaded?.Invoke(sceneName);
            onComplete?.Invoke();
        });
    }

    public bool TryUnloadActiveMiniGame(Action onComplete = null)
    {
        if (!HasActiveMiniGame)
        {
            _activeMiniGameSceneName = null;
            Debug.LogWarning("No active mini game scene is loaded.");
            return false;
        }

        return TryUnloadScene(_activeMiniGameSceneName, onComplete);
    }

    private bool TryLoadTrackedAdditiveScene(string sceneName, SceneVisibilityMode visibilityMode, Action onComplete)
    {
        return TryLoadScene(sceneName, LoadSceneMode.Additive, () =>
        {
            _activeAdditiveSceneName = sceneName;
            SetVisibilityMode(visibilityMode);
            onComplete?.Invoke();
        });
    }

    private void ClearActiveAdditiveScene()
    {
        _activeAdditiveSceneName = null;
    }

    private void ClearStaleActiveAdditiveScene()
    {
        if (string.IsNullOrWhiteSpace(_activeAdditiveSceneName) || HasActiveAdditiveScene)
        {
            return;
        }

        ClearActiveAdditiveScene();
    }

    private void SetVisibilityMode(SceneVisibilityMode visibilityMode)
    {
        if (CurrentVisibilityMode == visibilityMode)
        {
            return;
        }

        CurrentVisibilityMode = visibilityMode;
        VisibilityModeChanged?.Invoke(visibilityMode);
    }

    private bool CanUseSceneName(string sceneName)
    {
        if (!string.IsNullOrWhiteSpace(sceneName))
        {
            return true;
        }

        Debug.LogError("Scene name is empty.");
        return false;
    }

    private bool TryStartOperation(Func<AsyncOperation> createOperation, string operationName, Action onComplete)
    {
        if (IsBusy)
        {
            Debug.LogWarning($"Cannot {operationName} while another scene operation is running.");
            return false;
        }

        AsyncOperation operation;
        try
        {
            operation = createOperation();
        }
        catch (Exception exception)
        {
            Debug.LogError($"Could not {operationName}: {exception.Message}");
            return false;
        }

        if (operation == null)
        {
            Debug.LogError($"Could not {operationName}.");
            return false;
        }

        IsBusy = true;
        operation.completed += _ =>
        {
            IsBusy = false;
            onComplete?.Invoke();
        };

        return true;
    }
}
