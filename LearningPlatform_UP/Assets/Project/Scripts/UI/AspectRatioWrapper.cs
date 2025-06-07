using UnityEngine;
using UnityEngine.UI;

public class AspectRatioWrapper : MonoBehaviour
{
    private void Awake()
    {
        AspectRatioFitter aspectRatioFitter = GetComponent<AspectRatioFitter>();

        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect < 16f / 9f)
        {
            aspectRatioFitter.aspectRatio = 3f;
        }
        else
        {
            aspectRatioFitter.aspectRatio = 1f;
        }
    }
}
