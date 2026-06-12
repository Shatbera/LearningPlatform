using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Mission", menuName = "Scriptable Objects/Missions/Mission")]
public class MissionDefinitionSO : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private LocalizedString _title;
    [SerializeField] private Sprite _icon;
    [SerializeField] private LocalizedString _hint;
    [SerializeField] private List<MissionObjectiveDefinitionSO> _objectives = new();

    public string Id => _id;
    public LocalizedString Title => _title;
    public Sprite Icon => _icon;
    public LocalizedString Hint => _hint;
    public IReadOnlyList<MissionObjectiveDefinitionSO> Objectives => _objectives;
}
