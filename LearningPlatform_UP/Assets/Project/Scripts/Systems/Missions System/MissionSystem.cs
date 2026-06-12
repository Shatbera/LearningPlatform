using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization;

public class MissionSystem : IMissionSystem, ISaveable
{
    private readonly List<MissionRuntime> _missions;
    private readonly HashSet<string> _completedMissionIds = new();
    private int _currentMissionIndex;
    private MissionRuntime _activeMission;

    public MissionSystem(MissionCatalogSO catalog, MissionObjectiveContext context)
    {
        _missions = catalog == null
            ? new List<MissionRuntime>()
            : catalog.Missions
                .Where(mission => mission != null)
                .Select(mission => new MissionRuntime(mission, context))
                .ToList();

        _currentMissionIndex = FindFirstIncompleteMissionIndex();
        ActivateCurrentMission();
    }

    public string SaveKey => "missionSystem";
    public MissionRuntime CurrentMission => IsValidMissionIndex(_currentMissionIndex) ? _missions[_currentMissionIndex] : null;

    public event Action<MissionRuntime> CurrentMissionChanged;
    public event Action<MissionRuntime> MissionProgressChanged;
    public event Action<LocalizedString> HintRequested;

    public void RequestCurrentMissionHint()
    {
        if (CurrentMission != null)
        {
            HintRequested?.Invoke(CurrentMission.Definition.Hint);
        }
    }

    public object CaptureState()
    {
        MissionSystemSaveData saveData = new MissionSystemSaveData();
        saveData.CompletedMissionIds.AddRange(_completedMissionIds);

        foreach (var mission in _missions)
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
            MissionRuntime mission = _missions.FirstOrDefault(x => x.Definition.Id == progress.MissionId);
            mission?.RestoreObjectiveAmounts(progress.ObjectiveAmounts);
        }

        _currentMissionIndex = FindFirstIncompleteMissionIndex();
        ActivateCurrentMission();
        CurrentMissionChanged?.Invoke(CurrentMission);
        MissionProgressChanged?.Invoke(CurrentMission);
    }

    private void ActivateCurrentMission()
    {
        _activeMission = CurrentMission;
        if (_activeMission == null)
        {
            return;
        }

        _activeMission.ProgressChanged += OnActiveMissionProgressChanged;
        _activeMission.Completed += CompleteActiveMission;
        _activeMission.Start();

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
        MissionRuntime completedMission = CurrentMission;
        if (completedMission == null)
        {
            return;
        }

        DeactivateCurrentMission();
        _completedMissionIds.Add(completedMission.Definition.Id);
        _currentMissionIndex = FindFirstIncompleteMissionIndex();
        ActivateCurrentMission();

        CurrentMissionChanged?.Invoke(CurrentMission);
        MissionProgressChanged?.Invoke(CurrentMission);
    }

    private int FindFirstIncompleteMissionIndex()
    {
        for (int i = 0; i < _missions.Count; i++)
        {
            if (!_completedMissionIds.Contains(_missions[i].Definition.Id))
            {
                return i;
            }
        }

        return -1;
    }

    private bool IsValidMissionIndex(int index)
    {
        return index >= 0 && index < _missions.Count;
    }
}

[Serializable]
public class MissionSystemSaveData
{
    public List<string> CompletedMissionIds = new();
    public List<MissionProgressSaveData> MissionProgresses = new();
}

[Serializable]
public class MissionProgressSaveData
{
    public string MissionId;
    public List<int> ObjectiveAmounts = new();
}
