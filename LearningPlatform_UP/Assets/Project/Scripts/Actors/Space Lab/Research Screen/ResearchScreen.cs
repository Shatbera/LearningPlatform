using System.Collections;
using UnityEngine;

public class ResearchScreen : MonoBehaviour
{
    [SerializeField] private ResearchBeginEventChannel _researchBeginEventChannel;
    [SerializeField] private LocalizedTextSetter _sampleNameTxtSetter;
    [SerializeField] private LocalizedTextSetter _sampleInfoTxtSetter;
    private ResearchSampleSO _researchSample;
    private void OnEnable()
    {
        _researchBeginEventChannel.RegisterListener(OnResearchStarted);
        /*        _sampleNameTxt.text = "";
                _sampleInfoTxt.text = "";*/
        _sampleNameTxtSetter.SetRawText("");
        _sampleInfoTxtSetter.SetRawText("");
    }

    private void OnDisable()
    {
        _researchBeginEventChannel.UnregisterListener(OnResearchStarted);
    }

    private void OnResearchStarted(ResearchBeginEventData researchData)
    {
        _researchSample = researchData.ResearchSample;
        researchData.ResearchTask.Complete += OnResearchComplete;
        //_sampleNameTxt.text = _researchSample.SampleName;
        //_sampleInfoTxt.text = "...";
        _sampleInfoTxtSetter.SetRawText("...");
        _sampleNameTxtSetter.SetLocalizedText(_researchSample.LocalizedSampleName);
        StartCoroutine(ResearchCoroutine(researchData.ResearchTask));
    }

    private IEnumerator ResearchCoroutine(ResearchTask researchTask)
    {
        yield return new WaitUntil(() => researchTask.Progress > 0.5f);

        //string fullText = _researchSample.SampleInfo;
        string fullText = _researchSample.LocalizedSampleInfo.GetLocalizedString();
        int totalLength = fullText.Length;

        while (researchTask.Progress < 1f)
        {
            float normalized = Mathf.InverseLerp(0.5f, 1f, researchTask.Progress);
            int charsToShow = Mathf.FloorToInt(normalized * totalLength);
            //_sampleInfoTxt.text = fullText.Substring(0, charsToShow);
            _sampleInfoTxtSetter.SetRawText(fullText.Substring(0, charsToShow));
            yield return null;
        }

        //_sampleInfoTxt.text = fullText;
        _sampleInfoTxtSetter.SetRawText(fullText);
    }


    private void OnResearchComplete()
    {
        //_sampleInfoTxt.text = _researchSample.SampleInfo;
        _sampleInfoTxtSetter.SetLocalizedText(_researchSample.LocalizedSampleInfo);
        _researchSample = null;
    }
}
