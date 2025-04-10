using UnityEngine;

public class StarsParallax : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxFactor = 0.1f;

    private Vector3 lastPlayerPosition;

    void Start()
    {
        lastPlayerPosition = cameraTransform.position;
    }

    void FixedUpdate()
    {
        Vector3 delta = cameraTransform.position - lastPlayerPosition;
        transform.position += delta * parallaxFactor;
        lastPlayerPosition = cameraTransform.position;
    }
}
