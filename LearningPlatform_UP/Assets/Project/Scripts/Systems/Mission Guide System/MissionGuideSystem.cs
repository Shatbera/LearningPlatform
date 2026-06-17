using System;
using System.Collections.Generic;

public class MissionGuideSystem : IMissionGuideSystem, IDisposable
{
    private readonly Dictionary<MissionGuideTargetSO, MissionGuideTarget> _targets = new();
    private readonly IMissionSystem _missionSystem;

    public MissionGuideSystem(IEnumerable<MissionGuideTarget> targets, IMissionSystem missionSystem)
    {
        _missionSystem = missionSystem;

        if (targets != null)
        {
            foreach (MissionGuideTarget target in targets)
            {
                RegisterTarget(target);
            }
        }

        if (_missionSystem != null)
        {
            _missionSystem.CurrentMissionChanged += OnCurrentMissionChanged;
        }

        Refresh();
    }

    public MissionGuideTarget CurrentTarget { get; private set; }
    public event Action<MissionGuideTarget> CurrentTargetChanged;

    public void RegisterTarget(MissionGuideTarget target)
    {
        if (target == null || target.Definition == null)
        {
            return;
        }

        _targets[target.Definition] = target;
        Refresh();
    }

    public void UnregisterTarget(MissionGuideTarget target)
    {
        if (target == null || target.Definition == null)
        {
            return;
        }

        if (_targets.TryGetValue(target.Definition, out MissionGuideTarget registeredTarget) && registeredTarget == target)
        {
            _targets.Remove(target.Definition);
            Refresh();
        }
    }

    public bool TryGetTarget(MissionGuideTargetSO definition, out MissionGuideTarget target)
    {
        target = null;
        return definition != null && _targets.TryGetValue(definition, out target);
    }

    public void Refresh()
    {
        SetTargetFromMission(_missionSystem?.CurrentMission);
    }

    public void Dispose()
    {
        if (_missionSystem != null)
        {
            _missionSystem.CurrentMissionChanged -= OnCurrentMissionChanged;
        }
    }

    private void OnCurrentMissionChanged(MissionRuntime mission)
    {
        SetTargetFromMission(mission);
    }

    private void SetTargetFromMission(MissionRuntime mission)
    {
        if (mission == null || !mission.Definition.HasGuideTarget)
        {
            SetCurrentTarget(null);
            return;
        }

        SetCurrentTarget(TryGetTarget(mission.Definition.GuideTarget, out MissionGuideTarget target) ? target : null);
    }

    private void SetCurrentTarget(MissionGuideTarget target)
    {
        if (CurrentTarget == target)
        {
            return;
        }

        CurrentTarget = target;
        CurrentTargetChanged?.Invoke(CurrentTarget);
    }
}
