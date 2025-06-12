using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class LocalizedTextSetter : MonoBehaviour
{
    private TMP_Text _text;
    private LocalizedString _currentLocalizedString;
    public void SetLocalizedText(LocalizedString localizedString)
    {
        if (_currentLocalizedString != null)
        {
            _currentLocalizedString.StringChanged -= OnLocalizedTextChanged;
        }
        _currentLocalizedString = localizedString;

        _currentLocalizedString.StringChanged += OnLocalizedTextChanged;
        _currentLocalizedString.RefreshString();
    }

    private void OnEnable()
    {
        if(_currentLocalizedString != null)
        {
            _currentLocalizedString.StringChanged += OnLocalizedTextChanged;
            _currentLocalizedString.RefreshString();
        }
    }

    private void OnDisable()
    {
        if (_currentLocalizedString != null)
        {
            _currentLocalizedString.StringChanged -= OnLocalizedTextChanged;
        }
    }

    private void OnLocalizedTextChanged(string value)
    {
        if (_text == null)
        {
            _text = GetComponent<TMP_Text>();
        }
        _text.text = value;
    }
}
