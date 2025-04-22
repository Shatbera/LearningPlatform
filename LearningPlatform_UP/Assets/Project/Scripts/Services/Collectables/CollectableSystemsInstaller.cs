using System.Linq;
using UnityEngine;
public class CollectableSystemsInstaller : ServiceInstaller
{
    [SerializeField] private ItemsContainerBaseSO<ResearchSampleSO> _researchSamples;
    [SerializeField] private ResearchSampleSystemServiceRefSO _researchSystemService;
    public override void Install()
    {
        System.Collections.Generic.IEnumerable<(ResearchSampleSO item, ResearchSampleItemState)> itemStates = _researchSamples.Items.Select(item => (item, new ResearchSampleItemState()));
        _researchSystemService.InstallService(new CollectableItemSystem<ResearchSampleSO, ResearchSampleItemState>(itemStates));
    }
}
