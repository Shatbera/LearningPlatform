using UnityEngine;

[System.Serializable]
public class Particle
{
    public string Name;
    public GameObject ParticlePrefab;
}
public class ParticlesDataSO : ScriptableObject
{
    public Particle[] Particles;
}
