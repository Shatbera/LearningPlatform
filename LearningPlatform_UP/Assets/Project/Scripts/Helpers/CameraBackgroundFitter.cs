using Unity.Cinemachine;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
#endif
public class CameraBackgroundFitter : MonoBehaviour
{
    public SpriteRenderer background;
    public CinemachineCamera cam;
    void Start()
    {
        AdjustCameraSize();
    }

#if UNITY_EDITOR
    private void Update()
    {
        AdjustCameraSize();
    }

#endif
    void AdjustCameraSize()
    {

        float backgroundAspect = background.bounds.size.x / background.bounds.size.y;

        float screenAspect = (float)Screen.width / Screen.height;

        if (screenAspect >= backgroundAspect)
        {
            cam.Lens.OrthographicSize = (background.bounds.size.x / screenAspect) / 2;
        }
        else
        {
            cam.Lens.OrthographicSize = background.bounds.size.y / 2;
        }

    }
}
