using UnityEngine;
using System.Collections.Generic;

public class GrowPlantWaterEmitter : MonoBehaviour
{
    private readonly List<ParticleCollisionEvent> _collisionEvents = new();
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        GrowPlantWaterReceiver receiver = other.GetComponentInParent<GrowPlantWaterReceiver>();

        if (receiver == null)
        {
            return;
        }

        int collisionCount = _particleSystem == null
            ? 1
            : ParticlePhysicsExtensions.GetCollisionEvents(_particleSystem, other, _collisionEvents);

        receiver.ReceiveWater(collisionCount);
    }
}
