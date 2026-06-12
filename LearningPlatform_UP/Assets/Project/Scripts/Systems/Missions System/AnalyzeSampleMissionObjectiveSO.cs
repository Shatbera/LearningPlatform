using UnityEngine;

[CreateAssetMenu(fileName = "AnalyzeSampleObjective", menuName = "Scriptable Objects/Missions/Objectives/Analyze Sample")]
public class AnalyzeSampleMissionObjectiveSO : MissionObjectiveDefinitionSO
{
    [SerializeField] private ResearchSampleSO _sample;

    public ResearchSampleSO Sample => _sample;

    public override MissionObjectiveRuntime CreateRuntime(MissionObjectiveContext context)
    {
        return new AnalyzeSampleMissionObjectiveRuntime(this, context.ResearchBeginEventChannel);
    }
}

public class AnalyzeSampleMissionObjectiveRuntime : MissionObjectiveRuntime
{
    private readonly AnalyzeSampleMissionObjectiveSO _definition;
    private readonly ResearchBeginEventChannel _researchBeginEventChannel;
    private bool _isActive;

    public AnalyzeSampleMissionObjectiveRuntime(
        AnalyzeSampleMissionObjectiveSO definition,
        ResearchBeginEventChannel researchBeginEventChannel) : base(definition)
    {
        _definition = definition;
        _researchBeginEventChannel = researchBeginEventChannel;
    }

    public override void Start()
    {
        _isActive = true;
        _researchBeginEventChannel?.RegisterListener(OnResearchStarted);
    }

    public override void Stop()
    {
        _isActive = false;
        _researchBeginEventChannel?.UnregisterListener(OnResearchStarted);
    }

    private void OnResearchStarted(ResearchBeginEventData data)
    {
        if (_definition.Sample != null && data.ResearchSample.Id != _definition.Sample.Id)
        {
            return;
        }

        data.ResearchTask.Complete += OnResearchComplete;
    }

    private void OnResearchComplete()
    {
        if (_isActive)
        {
            AddProgress(1);
        }
    }
}
