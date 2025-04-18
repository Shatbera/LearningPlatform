using UnityEngine;

[CreateAssetMenu(fileName = "ResearchSample", menuName = "Scriptable Objects/ResearchSample")]
public class ResearchSampleSO : CollectableItemSO
{
    public string ID;
    public Sprite Sprite;
    public string SampleName;
    [TextArea]
    public string SampleInfo;

    public override string Id => ID;

    public override string DisplayName => SampleName;

    public override Sprite Icon => Sprite;
}
