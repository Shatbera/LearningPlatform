using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

public class ObjectLabel : MonoBehaviour
{
    private const float FADE_DURATION = 0.22f;

    [SerializeField] private TMP_Text _text;
    [SerializeField] private CanvasGroup _canvasGroup;

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetVisible(bool visible, Action onComplete = null) 
    {
        if(visible)
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, FADE_DURATION).OnComplete(()=>onComplete?.Invoke());
        }
        else
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.DOFade(0, FADE_DURATION).OnComplete(() => onComplete?.Invoke());
        }
    }
}
