using TMPro;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Localization;

public class ObjectLabel : MonoBehaviour
{
    private const float FADE_DURATION = 0.22f;

    [SerializeField] private LocalizedTextSetter _localizedTextSetter;
    [SerializeField] private CanvasGroup _canvasGroup;

    public void SetText(LocalizedString localizedText)
    {
        _localizedTextSetter.SetLocalizedText(localizedText);
    }
    public void SetVisible(bool visible, Action onComplete = null)
    {
        if (visible)
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, FADE_DURATION).OnComplete(() => onComplete?.Invoke());
        }
        else
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.DOFade(0, FADE_DURATION).OnComplete(() => onComplete?.Invoke());
        }
    }
}
