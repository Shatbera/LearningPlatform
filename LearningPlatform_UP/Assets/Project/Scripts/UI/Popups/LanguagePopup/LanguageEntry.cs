using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LanguageEntry : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string _languageCode;

    public static event Action<string> LanguageSelected;
    public void OnPointerClick(PointerEventData eventData)
    {
        LanguageSelected?.Invoke(_languageCode);
    }
}
