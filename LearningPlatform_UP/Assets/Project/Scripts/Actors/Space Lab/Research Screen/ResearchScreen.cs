using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResearchScreen : MonoBehaviour
{
    [SerializeField] private ResearchBeginEventChannel _researchBeginEventChannel;
    [SerializeField] private TMP_Text _sampleNameTxt;
    [SerializeField] private TMP_Text _sampleInfoTxt;
    private ResearchSampleSO _researchSample;
    private void OnEnable()
    {
        _researchBeginEventChannel.RegisterListener(OnResearchStarted);
        _sampleNameTxt.text = "";
        _sampleInfoTxt.text = "";
    }

    private void OnDisable()
    {
        _researchBeginEventChannel.UnregisterListener(OnResearchStarted);
    }

    private void OnResearchStarted(ResearchBeginEventData researchData)
    {
        _researchSample = researchData.ResearchSample;
        researchData.ResearchTask.Complete += OnResearchComplete;
        _sampleNameTxt.text = _researchSample.SampleName;
        _sampleInfoTxt.text = "...";
        StartCoroutine(ResearchCoroutine(researchData.ResearchTask));
    }

    private IEnumerator ResearchCoroutine(ResearchTask researchTask)
    {
        yield return new WaitUntil(() => researchTask.Progress > 0.5f);

        string fullText = _researchSample.SampleInfo;
        int totalLength = fullText.Length;

        while (researchTask.Progress < 1f)
        {
            float normalized = Mathf.InverseLerp(0.5f, 1f, researchTask.Progress);
            int charsToShow = Mathf.FloorToInt(normalized * totalLength);
            _sampleInfoTxt.text = fullText.Substring(0, charsToShow);

            yield return null;
        }

        _sampleInfoTxt.text = fullText;
    }


    private void OnResearchComplete()
    {
        _sampleInfoTxt.text = _researchSample.SampleInfo;
        _researchSample = null;
    }
}
