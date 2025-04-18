using UnityEngine;

public abstract class GameServiceRefBaseSO<TService> : ScriptableObject
{
    public TService Service { get; private set; }

    public void InstallService(TService service)
    {
        Service = service;
    }
}
