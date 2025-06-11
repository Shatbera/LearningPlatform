using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : Singleton<ParticlesManager>
{
    [SerializeField] private ParticlesDataSO _particlesData;
    private readonly Dictionary<string, Particle> _particlesDict = new();

    private readonly Dictionary<string, Queue<GameObject>> _spawnedParticlesDict = new();

    private const float PARTICLE_LIFETIME = 10f;
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

    public void SpawnParticle(string name, Vector3 spawnPos)
    {
        if (!_particlesDict.ContainsKey(name))
        {
            Debug.LogWarning($"Particle name '{name}' not found in particle dictionary.");
            return;
        }
        GameObject spawnedParticle = GetParticleObject(name);
        spawnedParticle.transform.position = spawnPos;
        StartCoroutine(ParticleDespawnCoroutine(name, spawnedParticle));
    }


    private GameObject GetParticleObject(string name)
    {
        if(_spawnedParticlesDict.ContainsKey(name) && _spawnedParticlesDict[name].Count > 0)
        {
            var obj = _spawnedParticlesDict[name].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return Instantiate(_particlesDict[name].ParticlePrefab, transform);
        }
    }

    private IEnumerator ParticleDespawnCoroutine(string name, GameObject particleObj)
    {
        yield return new WaitForSeconds(PARTICLE_LIFETIME);
        particleObj.SetActive(false);
        if (!_spawnedParticlesDict.ContainsKey(name))
        {
            _spawnedParticlesDict.Add(name, new Queue<GameObject>());
        }
        _spawnedParticlesDict[name].Enqueue(particleObj);
    }
}


