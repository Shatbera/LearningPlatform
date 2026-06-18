using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CollectableSystemsInstaller : ServiceInstaller
{
    [SerializeField] private ItemsContainerBaseSO<ResearchSampleSO> _researchSamples;
    [SerializeField] private ResearchSampleSystemServiceRefSO _researchSystemService;

    [SerializeField] private SaveSystemServiceRefSO _saveSystemRef;
    [SerializeField] private ItemCollectEventChannel _itemCollectEventChannel;


    public override void Install()
    {
        IEnumerable<(ResearchSampleSO item, ResearchSampleItemState)> itemStates = _researchSamples.Items.Select(item => (item, new ResearchSampleItemState(item.Id)));
        var researchSystem = new CollectableItemSystem<ResearchSampleSO, ResearchSampleItemState>(
            itemStates,
            saveKey: "collectableSystem",
            _itemCollectEventChannel);
        var worldCollectableStateSystem = new WorldCollectableStateSystem();

        _researchSystemService.InstallService(researchSystem);
        _saveSystemRef.Service.Register(researchSystem);
        _saveSystemRef.Service.Register(worldCollectableStateSystem);
    }
}
