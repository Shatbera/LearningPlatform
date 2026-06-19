using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

public class GameFlowSystem : ISaveable, IDisposable
{
    private readonly GameFlowSO _flow;
    private readonly IMissionSystem _missionSystem;
    private readonly RobotDialogueView _robotDialogueView;
    private readonly GameFlowEventChannel _eventChannel;
    private readonly MonoBehaviour _coroutineRunner;

    private Coroutine _flowCoroutine;
    private int _stepIndex;
    private bool _isPlaying;
    private bool _waitingForDialogueClose;
    private GameFlowEventSO _waitingForEvent;
    private bool _receivedWaitingEvent;

    public GameFlowSystem(
        GameFlowSO flow,
        IMissionSystem missionSystem,
        RobotDialogueView robotDialogueView,
        GameFlowEventChannel eventChannel,
        MonoBehaviour coroutineRunner)
    {
        _flow = flow;
        _missionSystem = missionSystem;
        _robotDialogueView = robotDialogueView;
        _eventChannel = eventChannel;
        _coroutineRunner = coroutineRunner;

        _eventChannel?.RegisterListener(OnGameFlowEventRaised);

        if (_robotDialogueView != null)
        {
            _robotDialogueView.Closed += OnRobotDialogueClosed;
        }
    }

    public string SaveKey => $"gameFlow:{(_flow == null ? "none" : _flow.Id)}";

    public void Play()
    {
        if (_isPlaying || _flow == null || _coroutineRunner == null)
        {
            return;
        }

        _isPlaying = true;
        _flowCoroutine = _coroutineRunner.StartCoroutine(RunFlow());
    }

    public void Stop()
    {
        if (_flowCoroutine != null && _coroutineRunner != null)
        {
            _coroutineRunner.StopCoroutine(_flowCoroutine);
        }

        _flowCoroutine = null;
        _isPlaying = false;
        _waitingForDialogueClose = false;
        _waitingForEvent = null;
        _receivedWaitingEvent = false;
    }

    public object CaptureState()
    {
        return new GameFlowSaveData
        {
            StepIndex = _stepIndex
        };
    }

    public void RestoreState(object data)
    {
        if (data is not GameFlowSaveData saveData)
        {
            return;
        }

        _stepIndex = Mathf.Max(0, saveData.StepIndex);
    }

    public void Dispose()
    {
        Stop();
        _eventChannel?.UnregisterListener(OnGameFlowEventRaised);

        if (_robotDialogueView != null)
        {
            _robotDialogueView.Closed -= OnRobotDialogueClosed;
        }
    }

    private IEnumerator RunFlow()
    {
        while (_flow != null && _stepIndex < _flow.Steps.Count)
        {
            GameFlowStep step = _flow.Steps[_stepIndex];
            if (step == null)
            {
                Advance();
                continue;
            }

            yield return RunStep(step);
            Advance();
        }

        _flowCoroutine = null;
        _isPlaying = false;
    }

    private IEnumerator RunStep(GameFlowStep step)
    {
        switch (step.Type)
        {
            case GameFlowStepType.WaitForEvent:
                yield return WaitForEvent(step.Event);
                break;
            case GameFlowStepType.Dialogue:
                yield return PlayDialogue(step.Dialogue);
                break;
            case GameFlowStepType.Delay:
                yield return new WaitForSeconds(step.DelaySeconds);
                break;
            case GameFlowStepType.StartMission:
                StartMission(step.Mission);
                break;
            case GameFlowStepType.WaitForMissionComplete:
                yield return WaitForMissionComplete(step.Mission);
                break;
            case GameFlowStepType.RaiseEvent:
                _eventChannel?.Raise(step.Event);
                break;
        }
    }

    private IEnumerator WaitForEvent(GameFlowEventSO flowEvent)
    {
        if (flowEvent == null)
        {
            yield break;
        }

        _waitingForEvent = flowEvent;
        _receivedWaitingEvent = false;

        while (!_receivedWaitingEvent)
        {
            yield return null;
        }

        _waitingForEvent = null;
        _receivedWaitingEvent = false;
    }

    private IEnumerator PlayDialogue(System.Collections.Generic.IReadOnlyList<LocalizedString> dialogue)
    {
        if (_robotDialogueView == null || dialogue == null || dialogue.Count == 0)
        {
            yield break;
        }

        for (int i = 0; i < dialogue.Count; i++)
        {
            _waitingForDialogueClose = true;
            _robotDialogueView.Show(dialogue[i]);

            while (_waitingForDialogueClose)
            {
                yield return null;
            }
        }
    }

    private void StartMission(MissionDefinitionSO mission)
    {
        if (mission == null || _missionSystem == null || _missionSystem.IsMissionCompleted(mission))
        {
            return;
        }

        _missionSystem.StartMission(mission);
    }

    private IEnumerator WaitForMissionComplete(MissionDefinitionSO mission)
    {
        if (mission == null || _missionSystem == null)
        {
            yield break;
        }

        while (!_missionSystem.IsMissionCompleted(mission))
        {
            yield return null;
        }
    }

    private void Advance()
    {
        _stepIndex++;
    }

    private void OnGameFlowEventRaised(GameFlowEvent flowEvent)
    {
        if (_waitingForEvent != null && flowEvent.Definition == _waitingForEvent)
        {
            _receivedWaitingEvent = true;
        }
    }

    private void OnRobotDialogueClosed()
    {
        _waitingForDialogueClose = false;
    }
}

[Serializable]
public class GameFlowSaveData
{
    public int StepIndex;
}
