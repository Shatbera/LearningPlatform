using System;
using UnityEngine;

public abstract class MissionObjectiveRuntime
{
    private int _currentAmount;

    protected MissionObjectiveRuntime(MissionObjectiveDefinitionSO definition)
    {
        Definition = definition;
    }

    public MissionObjectiveDefinitionSO Definition { get; }
    public int CurrentAmount => _currentAmount;
    public int TargetAmount => Definition.TargetAmount;
    public bool IsCompleted => CurrentAmount >= TargetAmount;

    public event Action ProgressChanged;

    public virtual void Start()
    {
    }

    public virtual void Stop()
    {
    }

    public void RestoreAmount(int amount)
    {
        _currentAmount = Mathf.Clamp(amount, 0, TargetAmount);
    }

    protected void AddProgress(int amount)
    {
        if (IsCompleted || amount <= 0)
        {
            return;
        }

        int nextAmount = Mathf.Clamp(_currentAmount + amount, 0, TargetAmount);
        if (nextAmount == _currentAmount)
        {
            return;
        }

        _currentAmount = nextAmount;
        ProgressChanged?.Invoke();
    }
}
