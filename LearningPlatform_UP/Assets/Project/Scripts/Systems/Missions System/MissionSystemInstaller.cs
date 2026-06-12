using UnityEngine;

public class MissionSystemInstaller : ServiceInstaller
{
    [SerializeField] private MissionCatalogSO _missionCatalog;
    [SerializeField] private MissionSystemRefSO _missionSystemRef;
    [SerializeField] private SaveSystemServiceRefSO _saveSystemRef;
    [SerializeField] private ItemCollectEventChannel _itemCollectEventChannel;
    [SerializeField] private ResearchBeginEventChannel _researchBeginEventChannel;

    public override void Install()
    {
        var context = new MissionObjectiveContext(_itemCollectEventChannel, _researchBeginEventChannel);
        var missionSystem = new MissionSystem(_missionCatalog, context);
        _missionSystemRef.InstallService(missionSystem);
        _saveSystemRef.Service.Register(missionSystem);
    }
}
