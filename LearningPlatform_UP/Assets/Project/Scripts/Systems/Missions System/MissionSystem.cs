using System;
using System.Collections.Generic;
using System.Linq;

public class MissionSystem : IMissionSystem, ISaveable
{
    private readonly List<MissionRuntime> _missions;
    private readonly HashSet<string> _completedMissionIds = new();
    private MissionRuntime _activeMission;
    private MissionRuntime _pendingMission;

    public MissionSystem(MissionCatalogSO catalog, MissionObjectiveContext context)
    {
        _missions = catalog == null
            ? new List<MissionRuntime>()
            : catalog.Missions
                .Where(mission => mission != null)
                .Select(mission => new MissionRuntime(mission, context))
                .ToList();

        QueueNextMission();
    }

    public string SaveKey => "missionSystem";
    public MissionRuntime CurrentMission => _activeMission;
    public MissionRuntime PendingMission => _pendingMission;

    public event Action<MissionRuntime> MissionOffered;
    public event Action<MissionRuntime> MissionStarted;
    public event Action<MissionRuntime> MissionCompleted;
    public event Action<MissionRuntime> CurrentMissionChanged;
    public event Action<MissionRuntime> MissionProgressChanged;

    public bool AcceptOfferedMission()
    {
        if (_pendingMission == null || _activeMission != null)
        {
            return false;
        }

        MissionRuntime mission = _pendingMission;
        _pendingMission = null;
        ActivateMission(mission);
        return true;
    }

    public object CaptureState()
    {
        var saveData = new MissionSystemSaveData
        {
            ActiveMissionId = _activeMission?.Definition.Id,
            PendingMissionId = _pendingMission?.Definition.Id
        };
        saveData.CompletedMissionIds.AddRange(_completedMissionIds);

        foreach (MissionRuntime mission in _missions)
        {
            saveData.MissionProgresses.Add(new MissionProgressSaveData
            {
                MissionId = mission.Definition.Id,
                ObjectiveAmounts = mission.CaptureObjectiveAmounts()
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
        _pendingMission = null;
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
            _pendingMission = FindIncompleteMission(saveData.PendingMissionId);
            if (_pendingMission == null)
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
        return string.IsNullOrEmpty(missionId)
            ? null
            : _missions.FirstOrDefault(mission => mission.Definition.Id == missionId);
    }

    private MissionRuntime FindIncompleteMission(string missionId)
    {
        return _completedMissionIds.Contains(missionId) ? null : FindMission(missionId);
    }

    private void QueueNextMission()
    {
        if (_activeMission != null || _pendingMission != null)
        {
            return;
        }

        _pendingMission = _missions.FirstOrDefault(
            mission => !_completedMissionIds.Contains(mission.Definition.Id));

        if (_pendingMission != null)
        {
            MissionOffered?.Invoke(_pendingMission);
        }
    }
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
