using System;
using UnityEngine.Localization;

public interface IMissionSystem
{
    MissionRuntime CurrentMission { get; }
    event Action<MissionRuntime> CurrentMissionChanged;
    event Action<MissionRuntime> MissionProgressChanged;
    event Action<LocalizedString> HintRequested;

    void RequestCurrentMissionHint();
}
