using System;
using System.Collections.Generic;
using UnityEngine.Localization;

public interface IMissionSystem
{
    MissionRuntime CurrentMission { get; }
    MissionRuntime PendingMission { get; }
    IReadOnlyList<LocalizedString> PendingMissionIntroDialogue { get; }
    event Action<MissionRuntime> MissionOffered;
    event Action<MissionRuntime> MissionStarted;
    event Action<MissionRuntime> MissionCompleted;
    event Action<MissionRuntime> CurrentMissionChanged;
    event Action<MissionRuntime> MissionProgressChanged;

    bool AcceptOfferedMission();
}
