using System.Collections.Generic;

public class SampleProgressionSystem
{
    private readonly ICollectableItemSystem<ResearchSampleSO, ResearchSampleItemState> _researchSamplesSystem;
    private readonly ResearchBeginEventChannel _researchBeginEventChannel;

    public SampleProgressionSystem(
        IEnumerable<ResearchSampleSO> samples,
        ICollectableItemSystem<ResearchSampleSO, ResearchSampleItemState> researchSamplesSystem,
        ResearchBeginEventChannel researchBeginEventChannel)
    {
        _researchSamplesSystem = researchSamplesSystem;
        _researchBeginEventChannel = researchBeginEventChannel;

        InitializeUnlockedSamples(samples);
        _researchBeginEventChannel?.RegisterListener(OnResearchStarted);
    }

    private void InitializeUnlockedSamples(IEnumerable<ResearchSampleSO> samples)
    {
        if (samples == null)
        {
            return;
        }

        foreach (ResearchSampleSO sample in samples)
        {
            if (sample == null || !sample.InitiallyUnlocked)
            {
                continue;
            }

            CollectableItemEntry<ResearchSampleSO, ResearchSampleItemState> entry = _researchSamplesSystem.GetItem(sample.Id);
            if (entry != null)
            {
                entry.State.SetUnlocked(true);
            }
        }
    }

    private void OnResearchStarted(ResearchBeginEventData data)
    {
        if (data.ResearchSample == null || data.ResearchTask == null)
        {
            return;
        }

        data.ResearchTask.Complete += () => OnResearchComplete(data.ResearchSample);
    }

    private void OnResearchComplete(ResearchSampleSO researchedSample)
    {
        CollectableItemEntry<ResearchSampleSO, ResearchSampleItemState> researchedEntry = _researchSamplesSystem.GetItem(researchedSample.Id);
        if (researchedEntry == null)
        {
            return;
        }

        researchedEntry.State.IsResearched = true;

        ResearchSampleSO sampleToUnlock = researchedSample.UnlocksWhenResearched;
        if (sampleToUnlock == null)
        {
            return;
        }

        CollectableItemEntry<ResearchSampleSO, ResearchSampleItemState> unlockEntry = _researchSamplesSystem.GetItem(sampleToUnlock.Id);
        if (unlockEntry != null)
        {
            unlockEntry.State.SetUnlocked(true);
        }
    }
}
