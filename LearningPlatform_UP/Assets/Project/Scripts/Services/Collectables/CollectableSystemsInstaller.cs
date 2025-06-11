using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CollectableSystemsInstaller : ServiceInstaller
{
    [SerializeField] private ItemsContainerBaseSO<ResearchSampleSO> _researchSamples;
    [SerializeField] private ResearchSampleSystemServiceRefSO _researchSystemService;

    [SerializeField] private SaveSystemServiceRefSO _saveSystemRef;


    public override void Install()
    {
        IEnumerable<(ResearchSampleSO item, ResearchSampleItemState)> itemStates = _researchSamples.Items.Select(item => (item, new ResearchSampleItemState(item.Id)));
        var researchSystem = new CollectableItemSystem<ResearchSampleSO, ResearchSampleItemState>(itemStates, saveKey: "collectableSystem");
        _researchSystemService.InstallService(researchSystem);
        _saveSystemRef.Service.Register(researchSystem);
    }
}
