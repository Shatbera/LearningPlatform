using UnityEngine;

[CreateAssetMenu(fileName = "MissionGuideTarget", menuName = "Scriptable Objects/Missions/Guide Target")]
public class MissionGuideTargetSO : ScriptableObject
{
    [SerializeField] private Sprite _icon;

    public Sprite Icon => _icon;
}
