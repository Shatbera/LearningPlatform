using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "ResearchSample", menuName = "Scriptable Objects/ResearchSample")]
public class ResearchSampleSO : CollectableItemSO
{
    public string ID;
    public Sprite Sprite;
    [SerializeField] private bool _initiallyUnlocked;
    [SerializeField] private ResearchSampleSO _unlocksWhenResearched;
    public string SampleName;
    public LocalizedString LocalizedSampleName;
    [TextArea]
    public string SampleInfo;
    public LocalizedString LocalizedSampleInfo;

    public override string Id => ID;
    public bool InitiallyUnlocked => _initiallyUnlocked;
    public ResearchSampleSO UnlocksWhenResearched => _unlocksWhenResearched;

    //public override string DisplayName => SampleName;
    public override LocalizedString LocalizedDisplayName => LocalizedSampleName;

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
    public bool IsResearched;
    public bool IsUnlocked;

    public event System.Action<bool> UnlockedChanged;

    public ResearchSampleItemState(string id, int initialAmount = 0) : base(id, initialAmount)
    {
    }

    public void SetUnlocked(bool isUnlocked)
    {
        if (IsUnlocked == isUnlocked)
        {
            return;
        }

        IsUnlocked = isUnlocked;
        UnlockedChanged?.Invoke(IsUnlocked);
    }
}
