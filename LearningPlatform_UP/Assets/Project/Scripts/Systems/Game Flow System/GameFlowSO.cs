using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "GameFlow", menuName = "Scriptable Objects/Game Flow/Game Flow")]
public class GameFlowSO : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private List<GameFlowStep> _steps = new();

    public string Id => string.IsNullOrWhiteSpace(_id) ? name : _id;
    public IReadOnlyList<GameFlowStep> Steps => _steps;
}

[Serializable]
public class GameFlowStep
{
    [SerializeField] private GameFlowStepType _type;
    [SerializeField] private GameFlowEventSO _event;
    [SerializeField] private float _delaySeconds;
    [SerializeField] private List<LocalizedString> _dialogue = new();
    [SerializeField] private MissionDefinitionSO _mission;

    public GameFlowStepType Type => _type;
    public GameFlowEventSO Event => _event;
    public float DelaySeconds => Mathf.Max(0f, _delaySeconds);
    public IReadOnlyList<LocalizedString> Dialogue => _dialogue;
    public MissionDefinitionSO Mission => _mission;
}

public enum GameFlowStepType
{
    WaitForEvent,
    Dialogue,
    Delay,
    StartMission,
    WaitForMissionComplete,
    RaiseEvent
}
