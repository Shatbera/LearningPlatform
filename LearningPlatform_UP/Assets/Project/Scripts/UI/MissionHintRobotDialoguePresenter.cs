using UnityEngine;
using UnityEngine.Localization;

public class MissionHintRobotDialoguePresenter : MonoBehaviour
{
    [SerializeField] private MissionSystemRefSO _missionSystemRef;
    [SerializeField] private RobotDialogueView _robotDialogueView;

    private IMissionSystem _missionSystem;

    private void OnEnable()
    {
        Bind();
    }

    private void Start()
    {
        Bind();
    }

    private void OnDisable()
    {
        if (_missionSystem != null)
        {
            _missionSystem.HintRequested -= OnHintRequested;
            _missionSystem = null;
        }
    }

    private void Bind()
    {
        if (_missionSystem != null || _missionSystemRef == null || _missionSystemRef.Service == null)
        {
            return;
        }

        _missionSystem = _missionSystemRef.Service;
        _missionSystem.HintRequested += OnHintRequested;
    }

    private void OnHintRequested(LocalizedString hint)
    {
        _robotDialogueView.Show(hint);
    }
}
