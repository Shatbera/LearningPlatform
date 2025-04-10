using UnityEngine;
using System.Collections;

[System.Serializable]
public class Soundtrack
{
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
}
public class MusicManager : MonoBehaviour
{
    public Soundtrack[] soundtracks;
    public float fadeDuration = 2f;
    public float delayBetweenTracks = 1f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(MusicLoopCoroutine());
    }

    private IEnumerator MusicLoopCoroutine()
    {
        while (true)
        {
            foreach(var track in soundtracks)
            {
                float length = track.clip.length;
                audioSource.clip = track.clip;
                audioSource.volume = track.volume;
                audioSource.Play();
                yield return new WaitForSeconds(length - fadeDuration);
                for (float t = 0; t < fadeDuration; t += Time.deltaTime)
                {
                    audioSource.volume = Mathf.Lerp(track.volume, 0, t / fadeDuration);
                    yield return null;
                }
                audioSource.Stop();
                yield return new WaitForSeconds(delayBetweenTracks);
            }
        }
    }
}
