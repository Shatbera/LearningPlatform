using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Particles")]
public class ParticlesDataSO : ScriptableObject
{
    public Particle[] Particles;
}

[System.Serializable]
public class Particle
{
    public string Name;
    public GameObject ParticlePrefab;
}

