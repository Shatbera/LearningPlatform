using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "MissionCatalog", menuName = "Scriptable Objects/Missions/Mission Catalog")]
public class MissionCatalogSO : ScriptableObject
{
    [SerializeField] private List<MissionCatalogStep> _steps = new();

    public IReadOnlyList<MissionCatalogStep> Steps => _steps;
}

[System.Serializable]
public class MissionCatalogStep
{
    [SerializeField] private MissionDefinitionSO _mission;
    [SerializeField] private List<LocalizedString> _introDialogue = new();

    public MissionDefinitionSO Mission => _mission;
    public IReadOnlyList<LocalizedString> IntroDialogue => _introDialogue;
}
