using UnityEngine;

public class MissionRobotDialoguePresenter : MonoBehaviour
{
    [SerializeField] private MissionSystemRefSO _missionSystemRef;
    [SerializeField] private RobotDialogueView _robotDialogueView;

    private IMissionSystem _missionSystem;
    private bool _waitingForMissionDialogue;

    private void OnEnable()
    {
        Bind();
    }

    private void Start()
    {
        Bind();
        ShowPendingMissionIfNeeded();
    }

    private void OnDisable()
    {
        if (_missionSystem != null)
        {
            _missionSystem.MissionOffered -= OnMissionOffered;
            _missionSystem = null;
        }

        if (_robotDialogueView != null)
        {
            _robotDialogueView.Closed -= OnDialogueClosed;
        }

        _waitingForMissionDialogue = false;
    }

    private void Bind()
    {
        if (_missionSystem != null || _missionSystemRef == null || _missionSystemRef.Service == null)
        {
            return;
        }

        _missionSystem = _missionSystemRef.Service;
        _missionSystem.MissionOffered += OnMissionOffered;

        if (_robotDialogueView != null)
        {
            _robotDialogueView.Closed += OnDialogueClosed;
        }
    }

    private void ShowPendingMissionIfNeeded()
    {
        if (_missionSystem?.PendingMission != null)
        {
            OnMissionOffered(_missionSystem.PendingMission);
        }
    }

    private void OnMissionOffered(MissionRuntime mission)
    {
        if (mission == null || _robotDialogueView == null)
        {
            return;
        }

        _waitingForMissionDialogue = true;
        _robotDialogueView.Show(mission.Definition.StartDialogue);
    }

    private void OnDialogueClosed()
    {
        if (!_waitingForMissionDialogue)
        {
            return;
        }

        _waitingForMissionDialogue = false;
        _missionSystem?.AcceptOfferedMission();
    }
}
