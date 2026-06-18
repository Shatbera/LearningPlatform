using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization;

public class MissionSystem : IMissionSystem, ISaveable
{
    private readonly List<MissionSequenceRuntimeStep> _steps;
    private readonly HashSet<string> _completedMissionIds = new();
    private MissionRuntime _activeMission;
    private MissionSequenceRuntimeStep _pendingStep;

    public MissionSystem(MissionCatalogSO catalog, MissionObjectiveContext context)
    {
        _steps = catalog == null
            ? new List<MissionSequenceRuntimeStep>()
            : catalog.Steps
                .Where(step => step?.Mission != null)
                .Select(step => new MissionSequenceRuntimeStep(step, new MissionRuntime(step.Mission, context)))
                .ToList();

        QueueNextMission();
    }

    public string SaveKey => "missionSystem";
    public MissionRuntime CurrentMission => _activeMission;
    public MissionRuntime PendingMission => _pendingStep?.Mission;
    public IReadOnlyList<LocalizedString> PendingMissionIntroDialogue =>
        _pendingStep?.IntroDialogue ?? Array.Empty<LocalizedString>();

    public event Action<MissionRuntime> MissionOffered;
    public event Action<MissionRuntime> MissionStarted;
    public event Action<MissionRuntime> MissionCompleted;
    public event Action<MissionRuntime> CurrentMissionChanged;
    public event Action<MissionRuntime> MissionProgressChanged;

    public bool AcceptOfferedMission()
    {
        if (_pendingStep == null || _activeMission != null)
        {
            return false;
        }

        MissionRuntime mission = _pendingStep.Mission;
        _pendingStep = null;
        ActivateMission(mission);
        return true;
    }

    public object CaptureState()
    {
        var saveData = new MissionSystemSaveData
        {
            ActiveMissionId = _activeMission?.Definition.Id,
            PendingMissionId = _pendingStep?.Mission.Definition.Id
        };
        saveData.CompletedMissionIds.AddRange(_completedMissionIds);

        foreach (MissionSequenceRuntimeStep step in _steps)
        {
            saveData.MissionProgresses.Add(new MissionProgressSaveData
            {
                MissionId = step.Mission.Definition.Id,
                ObjectiveAmounts = step.Mission.CaptureObjectiveAmounts()
            });
        }

        return saveData;
    }

    public void RestoreState(object data)
    {
        if (data is not MissionSystemSaveData saveData)
        {
            return;
        }

        DeactivateCurrentMission();
        _pendingStep = null;
        _completedMissionIds.Clear();

        foreach (string missionId in saveData.CompletedMissionIds)
        {
            if (!string.IsNullOrEmpty(missionId))
            {
                _completedMissionIds.Add(missionId);
            }
        }

        foreach (MissionProgressSaveData progress in saveData.MissionProgresses)
        {
            MissionRuntime mission = FindMission(progress.MissionId);
            mission?.RestoreObjectiveAmounts(progress.ObjectiveAmounts);
        }

        MissionRuntime activeMission = FindIncompleteMission(saveData.ActiveMissionId);
        if (activeMission != null)
        {
            ActivateMission(activeMission, notifyStarted: false);
        }
        else
        {
            _pendingStep = FindIncompleteStep(saveData.PendingMissionId);
            if (_pendingStep == null)
            {
                QueueNextMission();
            }
        }

        CurrentMissionChanged?.Invoke(CurrentMission);
        MissionProgressChanged?.Invoke(CurrentMission);
    }

    private void ActivateMission(MissionRuntime mission, bool notifyStarted = true)
    {
        _activeMission = mission;
        _activeMission.ProgressChanged += OnActiveMissionProgressChanged;
        _activeMission.Completed += CompleteActiveMission;
        _activeMission.Start();

        CurrentMissionChanged?.Invoke(_activeMission);
        MissionProgressChanged?.Invoke(_activeMission);

        if (notifyStarted)
        {
            MissionStarted?.Invoke(_activeMission);
        }

        if (_activeMission.IsCompleted)
        {
            CompleteActiveMission();
        }
    }

    private void DeactivateCurrentMission()
    {
        if (_activeMission == null)
        {
            return;
        }

        _activeMission.ProgressChanged -= OnActiveMissionProgressChanged;
        _activeMission.Completed -= CompleteActiveMission;
        _activeMission.Stop();
        _activeMission = null;
    }

    private void OnActiveMissionProgressChanged()
    {
        MissionProgressChanged?.Invoke(CurrentMission);
    }

    private void CompleteActiveMission()
    {
        MissionRuntime completedMission = _activeMission;
        if (completedMission == null)
        {
            return;
        }

        DeactivateCurrentMission();
        _completedMissionIds.Add(completedMission.Definition.Id);

        CurrentMissionChanged?.Invoke(null);
        MissionProgressChanged?.Invoke(null);
        MissionCompleted?.Invoke(completedMission);
        QueueNextMission();
    }

    private MissionRuntime FindMission(string missionId)
    {
        return FindStep(missionId)?.Mission;
    }

    private MissionRuntime FindIncompleteMission(string missionId)
    {
        return _completedMissionIds.Contains(missionId) ? null : FindMission(missionId);
    }

    private MissionSequenceRuntimeStep FindStep(string missionId)
    {
        return string.IsNullOrEmpty(missionId)
            ? null
            : _steps.FirstOrDefault(step => step.Mission.Definition.Id == missionId);
    }

    private MissionSequenceRuntimeStep FindIncompleteStep(string missionId)
    {
        return _completedMissionIds.Contains(missionId) ? null : FindStep(missionId);
    }

    private void QueueNextMission()
    {
        if (_activeMission != null || _pendingStep != null)
        {
            return;
        }

        _pendingStep = _steps.FirstOrDefault(
            step => !_completedMissionIds.Contains(step.Mission.Definition.Id));

        if (_pendingStep != null)
        {
            MissionOffered?.Invoke(_pendingStep.Mission);
        }
    }
}

internal class MissionSequenceRuntimeStep
{
    public MissionSequenceRuntimeStep(MissionCatalogStep definition, MissionRuntime mission)
    {
        Definition = definition;
        Mission = mission;
    }

    public MissionCatalogStep Definition { get; }
    public MissionRuntime Mission { get; }
    public IReadOnlyList<LocalizedString> IntroDialogue => Definition.IntroDialogue;
}

[Serializable]
public class MissionSystemSaveData
{
    public string ActiveMissionId;
    public string PendingMissionId;
    public List<string> CompletedMissionIds = new();
    public List<MissionProgressSaveData> MissionProgresses = new();
}

[Serializable]
public class MissionProgressSaveData
{
    public string MissionId;
    public List<int> ObjectiveAmounts = new();
}
