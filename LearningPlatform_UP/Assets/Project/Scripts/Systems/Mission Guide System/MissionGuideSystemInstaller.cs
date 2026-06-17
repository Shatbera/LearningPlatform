using UnityEngine;

public class MissionGuideSystemInstaller : ServiceInstaller
{
    [SerializeField] private MissionGuideTarget[] _targets;
    [SerializeField] private MissionSystemRefSO _missionSystemRef;
    [SerializeField] private MissionGuideSystemRefSO _missionGuideSystemRef;

    public override void Install()
    {
        MissionGuideTarget[] targets = _targets == null || _targets.Length == 0
            ? FindObjectsByType<MissionGuideTarget>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            : _targets;

        var missionGuideSystem = new MissionGuideSystem(targets, _missionSystemRef == null ? null : _missionSystemRef.Service);
        _missionGuideSystemRef.InstallService(missionGuideSystem);
    }
}
