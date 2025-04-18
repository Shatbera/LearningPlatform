using Unity.Cinemachine;
using UnityEngine;

public class SpaceObjectCameraHighlighter : SpaceObjectHighlighter
{
    [SerializeField] private CinemachineCamera _objectCamera;
    [SerializeField] private float minOrthoSize = 5f;
    [SerializeField] private float padding = 1f;
    public override void Highlight(IHighlightableSpaceObject highlightable)
    {
        base.Highlight(highlightable);
        _objectCamera.Follow = highlightable.Renderer.transform;
        _objectCamera.enabled = true;

        SpriteRenderer sr = highlightable.Renderer;
        if (sr != null && sr.sprite != null)
        {
            var s = sr.sprite;

            Vector2 spriteSize = s.rect.size / s.pixelsPerUnit;
            Vector2 borderTotal = new Vector2(s.border.x + s.border.z, s.border.y + s.border.w) / s.pixelsPerUnit;
            Vector2 effectiveSize = spriteSize - borderTotal;

            Vector3 lossyScale = sr.transform.lossyScale;
            Vector2 worldSize = new Vector2(
                effectiveSize.x * lossyScale.x,
                effectiveSize.y * lossyScale.y
            );

            float aspect = Camera.main.aspect;
            float requiredHeight = worldSize.y / 2f + padding;
            float requiredWidth = (worldSize.x / aspect) / 2f + padding;
            float requiredOrthoSize = Mathf.Max(requiredHeight, requiredWidth, minOrthoSize);

            _objectCamera.Lens.OrthographicSize = requiredOrthoSize;
        }
    }

    public override void Unhighlight(IHighlightableSpaceObject highlightable)
    {
        base.Unhighlight(highlightable);
        _objectCamera.Follow = null;
        _objectCamera.enabled = false;
    }
}
