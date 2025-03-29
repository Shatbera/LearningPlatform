using UnityEngine;

[CreateAssetMenu(fileName = "ObjectInfo", menuName = "ScriptableObjects/ObjectInfo")]
public class ObjectInfoSO : ScriptableObject
{
    [System.Serializable]
    public struct Fact
    {
        [TextArea]
        public string text;
    }

    public Fact[] Facts;
}
