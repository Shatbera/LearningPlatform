using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class RobotDialogueView : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private LocalizedTextSetter _messageText;
    [SerializeField] private Button _closeButton;

    private void OnEnable()
    {
        _closeButton?.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
        _closeButton?.onClick.RemoveListener(Hide);
    }

    public void Show(LocalizedString message)
    {
        SetVisible(true);
        _messageText.SetLocalizedText(message);
    }

    public void Show(string message)
    {
        SetVisible(true);
        _messageText.SetRawText(message);
    }

    public void Hide()
    {
        SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        if (_container != null)
        {
            _container.SetActive(visible);
        }
    }
}
