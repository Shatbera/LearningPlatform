using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemPickupAnimator : MonoBehaviour, IItemPickupAnimator
{
    [SerializeField] private RectTransform _backpackIconRect;
    [SerializeField] private Canvas _targetCanvas;
    [SerializeField] private ComponentPool<Image> _iconPool;
    [SerializeField] private AnimationCurve _moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private const float TARGET_ICON_SIZE = 150;
    private const float POP_DURATION = 0.15f;
    private const float ANIM_DURATION = 0.75f;
    public void AnimatePickup(IPickableItem item, Action onComplete = null)
    {
        StartCoroutine(PickupCoroutine(item, onComplete));
    }

    private IEnumerator PickupCoroutine(IPickableItem item, Action onComplete)
    {
        // Convert world position of sprite to canvas-local position
        Vector2 screenPos = Camera.main.WorldToScreenPoint(item.Renderer.transform.position);

        // Get a pooled icon
        Image icon = _iconPool.Get();
        icon.sprite = item.Renderer.sprite;
        RectTransform iconRect = icon.rectTransform;
        iconRect.position = screenPos;
        iconRect.localScale = Vector3.one;

        // Match the visual size of the SpriteRenderer
        Bounds bounds = item.Renderer.bounds;
        Vector2 screenSize = RectTransformUtility.WorldToScreenPoint(Camera.main, bounds.max) -
                             RectTransformUtility.WorldToScreenPoint(Camera.main, bounds.min);

        float scaleFactor = _targetCanvas.scaleFactor;
        iconRect.sizeDelta = screenSize / scaleFactor;

        // Pop animation (scale up then down)
        float popTime = 0f;
        Vector3 baseScale = Vector3.one;
        Vector3 popScale = Vector3.one * 1.35f;

        while (popTime < POP_DURATION)
        {
            popTime += Time.deltaTime;
            float t = popTime / POP_DURATION;
            iconRect.localScale = Vector3.Lerp(baseScale, popScale, Mathf.Sin(t * Mathf.PI));
            yield return null;
        }

        iconRect.localScale = Vector3.one;

        // Convert backpack icon position to canvas-local space
        Vector2 screenTarget = RectTransformUtility.WorldToScreenPoint(
            _targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _targetCanvas.worldCamera,
            _backpackIconRect.position
        );
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _targetCanvas.transform as RectTransform,
            screenTarget,
            _targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _targetCanvas.worldCamera,
            out Vector2 localTargetPos
        );

        // Move to backpack with curved arc
        Vector2 startPos = iconRect.anchoredPosition;
        float elapsed = 0f;
        float arcHeight = 100f;

        Vector2 startSize = iconRect.sizeDelta;
        Vector2 endSize = new Vector2(TARGET_ICON_SIZE, TARGET_ICON_SIZE);


        while (elapsed < ANIM_DURATION)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / ANIM_DURATION);
            float curvedT = _moveCurve.Evaluate(t);

            // Curved movement
            Vector2 pos = Vector2.Lerp(startPos, localTargetPos, curvedT);
            pos.y += Mathf.Sin(curvedT * Mathf.PI) * arcHeight;
            iconRect.anchoredPosition = pos;

            // Shrink to fixed size
            iconRect.sizeDelta = Vector2.Lerp(startSize, endSize, curvedT);

            yield return null;
        }

        _backpackIconRect.DOPunchScale(Vector3.one * 0.15f, 0.15f, 1);

        // Cleanup
        _iconPool.Release(icon);
        onComplete?.Invoke();
    }

    private float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);
}
