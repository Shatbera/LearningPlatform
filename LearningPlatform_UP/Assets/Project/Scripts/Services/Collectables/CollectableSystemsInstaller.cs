using UnityEngine;
public class CollectableSystemsInstaller : ServiceInstaller
{
    [SerializeField] private ItemsContainerBaseSO<ResearchSampleSO> _researchSamples;
    [SerializeField] private ResearchSampleSystemServiceRefSO _researchSystemService;
    public override void Install()
    {
        _researchSystemService.InstallService(new CollectableItemSystem<ResearchSampleSO>(_researchSamples.Items));
    }
}
