using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionCatalog", menuName = "Scriptable Objects/Missions/Mission Catalog")]
public class MissionCatalogSO : ScriptableObject
{
    [SerializeField] private List<MissionDefinitionSO> _missions = new();

    public IReadOnlyList<MissionDefinitionSO> Missions => _missions;
}
