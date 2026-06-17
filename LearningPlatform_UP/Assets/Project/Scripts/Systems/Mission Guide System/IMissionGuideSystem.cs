using System;

public interface IMissionGuideSystem
{
    MissionGuideTarget CurrentTarget { get; }
    event Action<MissionGuideTarget> CurrentTargetChanged;

    void RegisterTarget(MissionGuideTarget target);
    void UnregisterTarget(MissionGuideTarget target);
    bool TryGetTarget(MissionGuideTargetSO definition, out MissionGuideTarget target);
    void Refresh();
}
