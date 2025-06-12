using UnityEngine;
using TMPro;
using UnityEngine.Localization;

public class LocalizedFont : MonoBehaviour
{
    public TMP_Text text;
    public LocalizedAsset<TMP_FontAsset> localizedFont;

    void OnEnable()
    {
        localizedFont.AssetChanged += ChangeFont;
    }

    void OnDisable()
    {
        localizedFont.AssetChanged -= ChangeFont;
    }

    private void ChangeFont(TMP_FontAsset font)
    {
        text.font = font;
    }
}
