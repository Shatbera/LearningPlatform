using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LanguageEntry : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string _languageCode;

    private Action<string> _setLanguageAction;

    public void Setup(Action<string> setLanguageAction)
    {
        _setLanguageAction = setLanguageAction;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _setLanguageAction?.Invoke(_languageCode);
    }
}
