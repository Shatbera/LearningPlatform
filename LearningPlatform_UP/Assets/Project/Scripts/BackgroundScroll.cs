using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private float parralax = 2f;
    private Material mat;
    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void Update()
    {
        Vector2 offset = mat.mainTextureOffset;
        offset.x = transform.position.x / transform.localScale.x / parralax;
        offset.y = transform.position.y / transform.localScale.y / parralax;
        mat.mainTextureOffset = offset;
    }
}
