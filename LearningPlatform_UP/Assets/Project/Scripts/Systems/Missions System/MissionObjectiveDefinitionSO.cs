using UnityEngine;

public abstract class MissionObjectiveDefinitionSO : ScriptableObject
{
    [SerializeField, Min(1)] private int _targetAmount = 1;

    public int TargetAmount => _targetAmount;

    public abstract MissionObjectiveRuntime CreateRuntime(MissionObjectiveContext context);
}
