using System.Linq;
using UnityEngine;

public class LabSamplesPanel : MonoBehaviour
{
    [SerializeField] private ResearchSampleSystemServiceRefSO _researchSamplesRef;
    [SerializeField] private ComponentPool<LabSampleItem> _sampleItemPanelsPool;
    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        CollectableItemEntry<ResearchSampleSO, ResearchSampleItemState>[] collectedSamples = _researchSamplesRef.Service.GetAll().Where(x => x.State.Amount > 0 && !x.State.IsUnlocked).ToArray();
        for(int i = 0; i < collectedSamples.Length; i++)
        {
            var itemPanel = _sampleItemPanelsPool.Get();
            itemPanel.Setup(collectedSamples[i]);
        }
    }
}
