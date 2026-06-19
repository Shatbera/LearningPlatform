using UnityEngine;

public class GameFlowSystemInstaller : ServiceInstaller
{
    [SerializeField] private GameFlowSO _flow;
    [SerializeField] private MissionSystemRefSO _missionSystemRef;
    [SerializeField] private SaveSystemServiceRefSO _saveSystemRef;
    [SerializeField] private RobotDialogueView _robotDialogueView;
    [SerializeField] private GameFlowEventChannel _eventChannel;

    private GameFlowSystem _flowSystem;

    public override void Install()
    {
        _flowSystem = new GameFlowSystem(
            _flow,
            _missionSystemRef.Service,
            _robotDialogueView,
            _eventChannel,
            this);

        _saveSystemRef.Service.Register(_flowSystem);
    }

    private void Start()
    {
        _flowSystem?.Play();
    }

    private void OnDestroy()
    {
        if (_flowSystem == null)
        {
            return;
        }

        _saveSystemRef.Service?.Unregister(_flowSystem);
        _flowSystem.Dispose();
        _flowSystem = null;
    }
}
