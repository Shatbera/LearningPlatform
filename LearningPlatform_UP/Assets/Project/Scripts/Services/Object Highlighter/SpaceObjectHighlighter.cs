using UnityEngine;

public class SpaceObjectHighlighter : MonoBehaviour, IObjectHighlighter<IHighlightableSpaceObject>
{
    [SerializeField] private ComponentPool<ObjectLabel> _labelsPool;
    private ObjectLabel _ativeLabel;

    public virtual void Highlight(IHighlightableSpaceObject highlightable)
    {
        ObjectLabel label = _labelsPool.Get();
        _ativeLabel = label;

        label.SetVisible(true);
        label.SetText(highlightable.LabelName);

        var s = highlightable.Renderer.sprite;

        Vector2 spriteSize = s.rect.size / s.pixelsPerUnit;
        Vector2 borderTotal = new Vector2(s.border.x + s.border.z, s.border.y + s.border.w) / s.pixelsPerUnit;
        Vector2 effectiveSize = spriteSize - borderTotal;

        Vector3 lossyScale = highlightable.Renderer.transform.lossyScale;
        Vector2 worldSize = new Vector2(
            effectiveSize.x * lossyScale.x,
            effectiveSize.y * lossyScale.y
        );

        Vector2 worldPos = new Vector2(highlightable.Renderer.transform.position.x + worldSize.x / 2, highlightable.Renderer.transform.position.y);

        label.transform.position = worldPos;
    }

    public virtual void Unhighlight(IHighlightableSpaceObject highlightable)
    {
        if (_ativeLabel != null)
        {
            ObjectLabel label = _ativeLabel;
            label.SetVisible(false, onComplete: () => _labelsPool.Release(label));
            _ativeLabel = null;
        }
    }
}
