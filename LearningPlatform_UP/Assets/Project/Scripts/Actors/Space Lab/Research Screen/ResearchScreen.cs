using TMPro;
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
    }

    private void OnResearchComplete()
    {
        _sampleInfoTxt.text = _researchSample.SampleInfo;
        _researchSample = null;
    }
}
