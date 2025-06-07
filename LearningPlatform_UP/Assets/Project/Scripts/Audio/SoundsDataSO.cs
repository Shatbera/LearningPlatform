using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Sounds")]
public class SoundsDataSO : ScriptableObject
{
    public Sound[] Sounds;
}

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;
    [Range(0, 1)]
    public float Volume = 0.5f;
}
