using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier = 0.5f; // lower = slower
    private Transform cam;
    private Vector3 lastCamPosition;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPosition = cam.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - lastCamPosition;
        transform.position += new Vector3(delta.x * parallaxMultiplier, delta.y * parallaxMultiplier, 0);
        lastCamPosition = cam.position;
    }
}
