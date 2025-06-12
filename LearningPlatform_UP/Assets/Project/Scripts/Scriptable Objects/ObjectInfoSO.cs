using UnityEngine;
using UnityEngine.Localization;

public abstract class ObjectInfoSO : ScriptableObject
{
    [System.Serializable]
    public struct Fact
    {
        public LocalizedString LocalizedText;
    }

    public LocalizedString LocalizedObjectName;
    public Sprite ObjectSprite;
    public Fact[] Facts;
}
