using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticlesManager : Singleton<ParticlesManager>
{
    [SerializeField] private ParticlesDataSO _particlesData;
    private Dictionary<string, Particle> _particlesDict = new();

    [SerializeField] private ObjectPool<GameObject> _particlesPool;
    protected override void Awake()
    {
        base.Awake();
        InitializeParticles();
    }

    private void InitializeParticles()
    {
        foreach(var particle in _particlesData.Particles)
        {
            _particlesDict.Add(particle.Name, particle);
        }
    }

    public void SpawnParticle(string name, Vector3 spwanPos)
    {
        
    }
}


