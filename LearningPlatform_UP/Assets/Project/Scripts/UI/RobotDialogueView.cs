using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class RobotDialogueView : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private LocalizeStringEvent _messageText;
    [SerializeField] private Button _closeButton;
    private bool _isVisible;

    public event Action Closed;

    private void Awake()
    {
        SetVisible(false);
    }
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
        if (_messageText != null)
        {
            _messageText.StringReference = message;
            _messageText.RefreshString();
        }
    }

    public void Hide()
    {
        if (!_isVisible)
        {
            return;
        }

        SetVisible(false);
        Closed?.Invoke();
    }

    private void SetVisible(bool visible)
    {
        _isVisible = visible;
        if (_container != null)
        {
            _container.SetActive(visible);
        }
    }
}
