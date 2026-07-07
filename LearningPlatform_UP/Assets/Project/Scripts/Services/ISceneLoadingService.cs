using System;
using UnityEngine.SceneManagement;

public interface ISceneLoadingService
{
    event Action<string> MiniGameLoaded;
    event Action<string> MiniGameUnloaded;
    event Action<SceneVisibilityMode> VisibilityModeChanged;

    bool IsBusy { get; }
    string ActiveMiniGameSceneName { get; }
    bool HasActiveMiniGame { get; }
    SceneVisibilityMode CurrentVisibilityMode { get; }

    bool IsSceneLoaded(string sceneName);
    bool TryLoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, Action onComplete = null);
    bool TryLoadAdditiveScene(string sceneName, Action onComplete = null);
    bool TryLoadAdditiveScene(string sceneName, SceneVisibilityMode visibilityMode, Action onComplete = null);
    bool TryUnloadScene(string sceneName, Action onComplete = null);
    bool TryLoadMiniGame(string sceneName, Action onComplete = null);
    bool TryUnloadActiveMiniGame(Action onComplete = null);
}
