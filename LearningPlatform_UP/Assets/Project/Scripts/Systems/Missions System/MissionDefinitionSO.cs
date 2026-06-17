using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Mission", menuName = "Scriptable Objects/Missions/Mission")]
public class MissionDefinitionSO : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private LocalizedString _title;
    [SerializeField] private Sprite _icon;
    [SerializeField] private LocalizedString _startDialogue;
    [SerializeField] private MissionGuideTargetSO _guideTarget;
    [SerializeField] private List<MissionObjectiveDefinitionSO> _objectives = new();

    public string Id => _id;
    public LocalizedString Title => _title;
    public Sprite Icon => _icon;
    public LocalizedString StartDialogue => _startDialogue;
    public MissionGuideTargetSO GuideTarget => _guideTarget;
    public bool HasGuideTarget => _guideTarget != null;
    public IReadOnlyList<MissionObjectiveDefinitionSO> Objectives => _objectives;
}
