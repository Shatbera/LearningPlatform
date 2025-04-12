using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
#endif
public class CameraBackgroundFitter : MonoBehaviour
{
    public SpriteRenderer background;
    public Camera _camera;

    [Range(0f, 1f)]
    public float visibleAnchor = 1f; // 0 = top, 0.5 = center, 1 = bottom

    void Start()
    {
        FitCamera();
    }

#if UNITY_EDITOR
    void Update()
    {
        FitCamera();
    }
#endif

    void FitCamera()
    {
        float bgWidth = background.bounds.size.x;
        float bgHeight = background.bounds.size.y;
        Vector3 bgCenter = background.bounds.center;

        float screenAspect = (float)Screen.width / Screen.height;
        float bgAspect = bgWidth / bgHeight;

        float targetOrthoSize;

        if (screenAspect >= bgAspect)
        {
            // Fit width
            targetOrthoSize = (bgWidth / screenAspect) / 2f;
        }
        else
        {
            // Fit height
            targetOrthoSize = bgHeight / 2f;
        }

        _camera.orthographicSize = targetOrthoSize;

        float camHeight = 2f * targetOrthoSize;

        // Determine how much to shift camera based on anchor
        float minY = bgCenter.y - bgHeight / 2f + camHeight / 2f; // Align bottom
        float maxY = bgCenter.y + bgHeight / 2f - camHeight / 2f; // Align top

        float targetY = Mathf.Lerp(maxY, minY, visibleAnchor);

        _camera.transform.position = new Vector3(bgCenter.x, targetY, _camera.transform.position.z);
    }
}
