using System.Collections.Generic;
using UnityEngine;

public class MiniGamesPanel : MonoBehaviour
{
    [SerializeField] private SceneLoadingServiceRefSO _sceneLoadingServiceRef;
    [SerializeField] private List<MiniGameCard> _miniGameCards = new();
    [SerializeField] private Popup _popup;
    [SerializeField] private bool _closePopupOnPlay = true;

    private void Awake()
    {
        foreach (MiniGameCard card in _miniGameCards)
        {
            if (card == null)
            {
                continue;
            }

            card.Initialize(this);
        }
    }

    public void LoadMiniGame(string sceneName)
    {
        if (!TryGetSceneLoadingService(out ISceneLoadingService sceneLoadingService))
        {
            return;
        }

        bool startedLoading = sceneLoadingService.TryLoadMiniGame(sceneName);
        if (startedLoading && _closePopupOnPlay && _popup != null)
        {
            _popup.Close();
        }
    }

    public void UnloadMiniGame()
    {
        if (!TryGetSceneLoadingService(out ISceneLoadingService sceneLoadingService))
        {
            return;
        }

        sceneLoadingService.TryUnloadActiveMiniGame();
    }

    private bool TryGetSceneLoadingService(out ISceneLoadingService sceneLoadingService)
    {
        sceneLoadingService = null;
        if (_sceneLoadingServiceRef == null)
        {
            Debug.LogError($"{nameof(MiniGamesPanel)} has no scene loading service ref assigned.", this);
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
