using UnityEngine;
using Unity.Cinemachine;

public class ObjectsHighlightController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _objectCamera;
    [SerializeField] private float minOrthoSize = 5f;
    [SerializeField] private float padding = 1f;

    [SerializeField] private ComponentPool<ObjectLabel> _labelsPool;
    private ObjectLabel _ativeLabel;
    private void OnEnable()
    {
        IHighlightableObject.Highlighted += HighlightObject;
    }

    private void OnDisable()
    {
        IHighlightableObject.Highlighted -= HighlightObject;
    }

    private void HighlightObject(IHighlightableObject obj, bool highlight)
    {
        HighlightWithLabel(obj, highlight);
        HighlightWithCamera(obj, highlight);
    }

    private void HighlightWithLabel(IHighlightableObject obj, bool highlight)
    {
        if (highlight)
        {
            ObjectLabel label = _labelsPool.Get();
            _ativeLabel = label;
            
            label.SetVisible(true);
            label.SetText(obj.LabelName);

            var s = obj.Renderer.sprite;

            Vector2 spriteSize = s.rect.size / s.pixelsPerUnit;
            Vector2 borderTotal = new Vector2(s.border.x + s.border.z, s.border.y + s.border.w) / s.pixelsPerUnit;
            Vector2 effectiveSize = spriteSize - borderTotal;

            Vector3 lossyScale = obj.Renderer.transform.lossyScale;
            Vector2 worldSize = new Vector2(
                effectiveSize.x * lossyScale.x,
                effectiveSize.y * lossyScale.y
            );

            Vector2 worldPos = new Vector2(obj.Renderer.transform.position.x + worldSize.x / 2, obj.Renderer.transform.position.y);

            label.transform.position = worldPos;
        }
        else
        {
            if(_ativeLabel != null)
            {
                ObjectLabel label = _ativeLabel;
                label.SetVisible(false, onComplete: () => _labelsPool.Release(label));
                _ativeLabel = null;
            }
        }

    }

    private void HighlightWithCamera(IHighlightableObject obj, bool highlight)
    {
        if (highlight)
        {
            _objectCamera.Follow = obj.Renderer.transform;
            _objectCamera.enabled = true;

            SpriteRenderer sr = obj.Renderer;
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
        else
        {
            _objectCamera.Follow = null;
            _objectCamera.enabled = false;
        }
    }
}
