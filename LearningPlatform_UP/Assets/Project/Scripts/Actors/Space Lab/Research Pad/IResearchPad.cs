public interface IResearchPad
{
    public bool TryShowSamplePreview(ResearchSampleSO sample);
    public bool TryHideSamplePreview(ResearchSampleSO sample);
    public bool TryPlaceSample(ResearchSampleSO sample);
}
