using System;
using System.Collections.Generic;
using System.Linq;

public class MissionSystem : IMissionSystem, ISaveable
{
    private readonly List<MissionRuntime> _missions;
    private readonly HashSet<string> _completedMissionIds = new();
    private MissionRuntime _activeMission;

    public MissionSystem(MissionCatalogSO catalog, MissionObjectiveContext context)
    {
        _missions = catalog == null
            ? new List<MissionRuntime>()
            : catalog.Missions
                .Where(mission => mission != null)
                .GroupBy(mission => mission.Id)
                .Select(group => new MissionRuntime(group.First(), context))
                .ToList();
    }

    public string SaveKey => "missionSystem";
    public MissionRuntime CurrentMission => _activeMission;

    public event Action<MissionRuntime> MissionStarted;
    public event Action<MissionRuntime> MissionCompleted;
    public event Action<MissionRuntime> CurrentMissionChanged;
    public event Action<MissionRuntime> MissionProgressChanged;

    public bool StartMission(MissionDefinitionSO definition)
    {
        return definition != null && StartMission(definition.Id);
    }

    public bool StartMission(string missionId)
    {
        MissionRuntime mission = FindMission(missionId);
        if (mission == null || IsMissionCompleted(missionId))
        {
            return false;
        }

        if (_activeMission != null)
        {
            return _activeMission.Definition.Id == mission.Definition.Id;
        }

        ActivateMission(mission);
        return true;
    }

    public bool IsMissionCompleted(MissionDefinitionSO definition)
    {
        return definition != null && IsMissionCompleted(definition.Id);
    }

    public bool IsMissionCompleted(string missionId)
    {
        return !string.IsNullOrEmpty(missionId) && _completedMissionIds.Contains(missionId);
    }

    public object CaptureState()
    {
        var saveData = new MissionSystemSaveData
        {
            ActiveMissionId = _activeMission?.Definition.Id
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
}

[Serializable]
public class MissionSystemSaveData
{
    public string ActiveMissionId;
    public List<string> CompletedMissionIds = new();
    public List<MissionProgressSaveData> MissionProgresses = new();
}

[Serializable]
public class MissionProgressSaveData
{
    public string MissionId;
    public List<int> ObjectiveAmounts = new();
}
