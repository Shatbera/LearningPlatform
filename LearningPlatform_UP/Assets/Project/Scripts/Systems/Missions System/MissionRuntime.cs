using System;
using System.Collections.Generic;
using System.Linq;

public class MissionRuntime
{
    private readonly List<MissionObjectiveRuntime> _objectives;

    public MissionRuntime(MissionDefinitionSO definition, MissionObjectiveContext context)
    {
        Definition = definition;
        _objectives = definition.Objectives
            .Where(objective => objective != null)
            .Select(objective => objective.CreateRuntime(context))
            .Where(objective => objective != null)
            .ToList();
    }

    public MissionDefinitionSO Definition { get; }
    public IReadOnlyList<MissionObjectiveRuntime> Objectives => _objectives;
    public int CurrentAmount => _objectives.Sum(objective => objective.CurrentAmount);
    public int TargetAmount => _objectives.Sum(objective => objective.TargetAmount);
    public bool IsCompleted => _objectives.Count == 0 || _objectives.All(objective => objective.IsCompleted);

    public event Action ProgressChanged;
    public event Action Completed;

    public void Start()
    {
        foreach (var objective in _objectives)
        {
            objective.ProgressChanged += OnObjectiveProgressChanged;
            objective.Start();
        }
    }

    public void Stop()
    {
        foreach (var objective in _objectives)
        {
            objective.ProgressChanged -= OnObjectiveProgressChanged;
            objective.Stop();
        }
    }

    public List<int> CaptureObjectiveAmounts()
    {
        return _objectives.Select(objective => objective.CurrentAmount).ToList();
    }

    public void RestoreObjectiveAmounts(IReadOnlyList<int> amounts)
    {
        if (amounts == null)
        {
            return;
        }

        for (int i = 0; i < _objectives.Count && i < amounts.Count; i++)
        {
            _objectives[i].RestoreAmount(amounts[i]);
        }
    }

    private void OnObjectiveProgressChanged()
    {
        ProgressChanged?.Invoke();

        if (IsCompleted)
        {
            Completed?.Invoke();
        }
    }
}
