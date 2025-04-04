using Unity.Cinemachine;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
#endif
public class CameraBackgroundFitter : MonoBehaviour
{
    public SpriteRenderer background;
    public CinemachineCamera[] cameras;
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
            SetView((background.bounds.size.x / screenAspect) / 2);
        }
        else
        {
            SetView(background.bounds.size.y / 2);
        }

    }

    private void SetView(float orthoSize)
    {
        foreach(var cam in cameras)
        {
            cam.Lens.OrthographicSize = orthoSize;
        }
    }
}
