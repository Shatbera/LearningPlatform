using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "Scriptable Objects/Planet")]
public class PlanetSO : ExplorableObjectSO
{
    public string Id;
    public bool InitiallyLocked;

    public string SaveId => string.IsNullOrEmpty(Id) ? name : Id;
}
