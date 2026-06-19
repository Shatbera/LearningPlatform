using UnityEngine;

[CreateAssetMenu(fileName = "GameFlowEvent", menuName = "Scriptable Objects/Game Flow/Game Flow Event")]
public class GameFlowEventSO : ScriptableObject
{
    [SerializeField] private string _id;

    public string Id => string.IsNullOrWhiteSpace(_id) ? name : _id;
}
