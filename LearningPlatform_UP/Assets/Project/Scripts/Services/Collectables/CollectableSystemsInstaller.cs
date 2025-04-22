using System.Linq;
using UnityEngine;
public class CollectableSystemsInstaller : ServiceInstaller
{
    [SerializeField] private ItemsContainerBaseSO<ResearchSampleSO> _researchSamples;
    [SerializeField] private ResearchSampleSystemServiceRefSO _researchSystemService;

    [SerializeField] private SaveSystemServiceRefSO _saveSystemRef;

    private const string RESEARCH_SAMPLES_SYSTEM_SAVE_KEY = "researchSamplesSystem";
    public override void Install()
    {
        System.Collections.Generic.IEnumerable<(ResearchSampleSO item, ResearchSampleItemState)> itemStates = _researchSamples.Items.Select(item => (item, new ResearchSampleItemState(item.Id)));
        var researchSystem = new CollectableItemSystem<ResearchSampleSO, ResearchSampleItemState>(itemStates, saveKey: RESEARCH_SAMPLES_SYSTEM_SAVE_KEY);
        _researchSystemService.InstallService(researchSystem);
        _saveSystemRef.Service.Register(researchSystem);
    }
}
