using System;

public interface IMissionSystem
{
    MissionRuntime CurrentMission { get; }
    event Action<MissionRuntime> MissionStarted;
    event Action<MissionRuntime> MissionCompleted;
    event Action<MissionRuntime> CurrentMissionChanged;
    event Action<MissionRuntime> MissionProgressChanged;

    bool StartMission(MissionDefinitionSO definition);
    bool StartMission(string missionId);
    bool IsMissionCompleted(MissionDefinitionSO definition);
    bool IsMissionCompleted(string missionId);
}
