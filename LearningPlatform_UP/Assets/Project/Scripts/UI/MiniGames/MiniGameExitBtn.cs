using UnityEngine;

public class MiniGameExitBtn : MonoBehaviour
{
    [SerializeField] private SceneLoadingServiceRefSO _sceneLoadingServiceRef;
    [SerializeField] private string _miniGameSceneName;

    public void ExitBtn()
    {
        if (!TryGetSceneLoadingService(out ISceneLoadingService sceneLoadingService))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(_miniGameSceneName))
        {
            sceneLoadingService.TryUnloadActiveMiniGame();
            return;
        }

        sceneLoadingService.TryUnloadScene(_miniGameSceneName);
    }

    private bool TryGetSceneLoadingService(out ISceneLoadingService sceneLoadingService)
    {
        sceneLoadingService = null;
        if (_sceneLoadingServiceRef == null)
        {
            Debug.LogError($"{nameof(MiniGameExitBtn)} has no scene loading service ref assigned.", this);
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
