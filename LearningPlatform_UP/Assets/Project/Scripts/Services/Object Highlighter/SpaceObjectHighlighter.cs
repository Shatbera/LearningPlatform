using UnityEngine;
using System.Collections.Generic;

public class SpaceObjectHighlighter : MonoBehaviour, IObjectHighlighter<IHighlightableSpaceObject>
{
    [SerializeField] private ComponentPool<ObjectLabel> _labelsPool;
    [SerializeField] private bool Follow;

    private Dictionary<IHighlightableSpaceObject, ObjectLabel> _activeLabels = new();

    public virtual void Highlight(IHighlightableSpaceObject highlightable)
    {
        if (_activeLabels.ContainsKey(highlightable)) return;

        ObjectLabel label = _labelsPool.Get();
        label.SetVisible(true);
        label.SetText(highlightable.LabelName);
        _activeLabels.Add(highlightable, label);

        UpdateLabelPosition(highlightable, label);
    }

    public virtual void Unhighlight(IHighlightableSpaceObject highlightable)
    {
        if (_activeLabels.TryGetValue(highlightable, out var label))
        {
            label.SetVisible(false, onComplete: () => _labelsPool.Release(label));
            _activeLabels.Remove(highlightable);
        }
    }

    private void Update()
    {
        if (!Follow) return;

        foreach (var kvp in _activeLabels)
        {
            UpdateLabelPosition(kvp.Key, kvp.Value);
        }
    }

    private void UpdateLabelPosition(IHighlightableSpaceObject target, ObjectLabel label)
    {
        var s = target.Renderer.sprite;

        Vector2 spriteSize = s.rect.size / s.pixelsPerUnit;
        Vector2 borderTotal = new Vector2(s.border.x + s.border.z, s.border.y + s.border.w) / s.pixelsPerUnit;
        Vector2 effectiveSize = spriteSize - borderTotal;

        Vector3 lossyScale = target.Renderer.transform.lossyScale;
        Vector2 worldSize = new Vector2(
            effectiveSize.x * lossyScale.x,
            effectiveSize.y * lossyScale.y
        );

        Vector2 worldPos = new Vector2(target.Renderer.transform.position.x + worldSize.x / 2, target.Renderer.transform.position.y);

        label.transform.position = worldPos;
    }
}
