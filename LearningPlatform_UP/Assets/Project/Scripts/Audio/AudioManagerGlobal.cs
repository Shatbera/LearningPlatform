using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioManagerGlobal : Singleton<AudioManagerGlobal>
{
    [SerializeField] private SoundsDataSO _soundsData;
    [SerializeField] private AudioSource _source;
    private readonly Dictionary<string, Sound> _soundsDict = new Dictionary<string, Sound>();

    protected override void Awake()
    {
        base.Awake();
        SetupSounds();
    }
    private void SetupSounds()
    {
        foreach(var sound in _soundsData.Sounds)
        {
            _soundsDict.Add(sound.Name, sound);
        }
    }
    public void PlayOneShot(string name)
    {
        var sound = _soundsDict[name];
        _source.PlayOneShot(sound.Clip, sound.Volume);
    }
}
