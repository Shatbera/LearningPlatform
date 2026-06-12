public readonly struct MissionObjectiveContext
{
    public readonly ItemCollectEventChannel ItemCollectEventChannel;
    public readonly ResearchBeginEventChannel ResearchBeginEventChannel;

    public MissionObjectiveContext(
        ItemCollectEventChannel itemCollectEventChannel,
        ResearchBeginEventChannel researchBeginEventChannel)
    {
        ItemCollectEventChannel = itemCollectEventChannel;
        ResearchBeginEventChannel = researchBeginEventChannel;
    }
}
