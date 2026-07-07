using UnityEngine;
using UnityEngine.UI;

public class MiniGameCard : MonoBehaviour
{
    [SerializeField] private string _miniGameSceneName;
    [SerializeField] private Button _playButton;

    private MiniGamesPanel _miniGamesPanel;

    public void Initialize(MiniGamesPanel miniGamesPanel)
    {
        _miniGamesPanel = miniGamesPanel;
    }

    private void Awake()
    {
        if (_playButton != null)
        {
            _playButton.onClick.AddListener(PlayBtn);
        }
    }

    private void OnDestroy()
    {
        if (_playButton != null)
        {
            _playButton.onClick.RemoveListener(PlayBtn);
        }
    }

    public void PlayBtn()
    {
        if (_miniGamesPanel == null)
        {
            Debug.LogError($"Mini game card {name} has no mini games panel assigned.", this);
            return;
        }

        if (string.IsNullOrWhiteSpace(_miniGameSceneName))
        {
            Debug.LogError($"Mini game card {name} has no scene name assigned.", this);
            return;
        }

        _miniGamesPanel.LoadMiniGame(_miniGameSceneName);
    }
}
