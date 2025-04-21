using System.Collections;
using UnityEngine;

public class ResearchPad : MonoBehaviour, IResearchPad
{
    [SerializeField] private SpriteRenderer _sampleRenderer;
    [SerializeField] private ResearchBeginEventChannel _researchBeginEventChannel;

    private const float PREVIEW_ALPHA = 0.5f;
    private const float RESEARCH_DURATION = 2f;

    private ResearchSampleSO _placedSample;

    private void Awake()
    {
        _sampleRenderer.sprite = null;
    }
    public bool TryHideSamplePreview(ResearchSampleSO sample)
    {
        if (_placedSample != null)
        {
            return false;
        }
        _sampleRenderer.sprite = null;
        return true;
    }

    public bool TryPlaceSample(ResearchSampleSO sample)
    {
        if (_placedSample != null)
        {
            return false;
        }
        _placedSample = sample;
        _sampleRenderer.sprite = sample.Icon;
        Color tmp = _sampleRenderer.color;
        tmp.a = 1;
        _sampleRenderer.color = tmp;
        StartCoroutine(ResearchCoroutine());
        return true;
    }

    public bool TryShowSamplePreview(ResearchSampleSO sample)
    {
        if(_placedSample != null )
        {
            return false;
        }
        _sampleRenderer.sprite = sample.Icon;
        Color tmp = _sampleRenderer.color;
        tmp.a = PREVIEW_ALPHA;
        _sampleRenderer.color = tmp;
        return true;
    }

    public IEnumerator ResearchCoroutine()
    {
        ResearchTask researchTask = new ResearchTask();
        _researchBeginEventChannel.Raise(new ResearchBeginEventData
        {
            ResearchSample = _placedSample,
            ResearchTask = researchTask
        });
        for(float t = 0; t < RESEARCH_DURATION; t += Time.deltaTime)
        {
            researchTask.Progress = t / RESEARCH_DURATION;
            yield return null;
        }
        researchTask.Progress = 1;
        researchTask.RaiseComplete();
        _placedSample = null;
    }
}
