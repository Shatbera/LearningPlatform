using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CollectableSystemsInstaller : ServiceInstaller
{
    public enum World { Space, Ocean }

    [SerializeField] private World _world;
    [SerializeField] private ItemsContainerBaseSO<ResearchSampleSO> _researchSamples;
    [SerializeField] private ResearchSampleSystemServiceRefSO _researchSystemService;

    [SerializeField] private SaveSystemServiceRefSO _saveSystemRef;

    private readonly Dictionary<World, string> SaveKeysDict = new()
    {
        { World.Space, "researchSamplesSystem" },
        { World.Ocean, "oceanSamplesSystem" },
    };

    public override void Install()
    {
        IEnumerable<(ResearchSampleSO item, ResearchSampleItemState)> itemStates = _researchSamples.Items.Select(item => (item, new ResearchSampleItemState(item.Id)));
        var researchSystem = new CollectableItemSystem<ResearchSampleSO, ResearchSampleItemState>(itemStates, saveKey: SaveKeysDict[_world]);
        _researchSystemService.InstallService(researchSystem);
        _saveSystemRef.Service.Register(researchSystem);
    }
}
