using UnityEngine;
using DG.Tweening;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fadeGroup;
    private const float FADE_DURATION = 1f;
    public void Fade(bool fade, float duration = FADE_DURATION, System.Action onComplete = null)
    {
        if (fade)
        {
            _fadeGroup.alpha= 0;
            _fadeGroup.gameObject.SetActive(true);
            _fadeGroup.DOFade(1, duration).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
        else
        {
            _fadeGroup.alpha= 1;
            _fadeGroup.DOFade(0, duration).OnComplete(() =>
            {
                _fadeGroup.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }
    }
}
