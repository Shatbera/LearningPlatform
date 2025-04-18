using UnityEngine;

[CreateAssetMenu(fileName = "ResearchSample", menuName = "Scriptable Objects/ResearchSample")]
public class ResearchSampleSO : CollectableItemSO
{
    public Sprite Sprite;
    public string SampleName;
    [TextArea]
    public string SampleInfo;
}
