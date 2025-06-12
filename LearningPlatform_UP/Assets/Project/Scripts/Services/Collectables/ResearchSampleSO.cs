using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "ResearchSample", menuName = "Scriptable Objects/ResearchSample")]
public class ResearchSampleSO : CollectableItemSO
{
    public string ID;
    public Sprite Sprite;
    public string SampleName;
    public LocalizedString LocalizedSampleName;
    [TextArea]
    public string SampleInfo;
    public LocalizedString LocalizedSampleInfo;

    public override string Id => ID;

    public override string DisplayName => SampleName;

    public override Sprite Icon => Sprite;

    private void OnValidate()
    {
        /*ID = name.Replace(" - ", "_");
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();*/
    }
}

[System.Serializable]
public class ResearchSampleItemState : CollectableItemState
{
    public bool IsUnlocked;
    public ResearchSampleItemState(string id, int initialAmount = 0) : base(id, initialAmount)
    {
    }
}
