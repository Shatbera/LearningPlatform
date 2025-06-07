using UnityEngine;

public abstract class ObjectInfoSO : ScriptableObject
{
    [System.Serializable]
    public struct Fact
    {
        [TextArea]
        public string text;
    }

    public string ObjectName;
    public Sprite ObjectSprite;
    public Fact[] Facts;
}
