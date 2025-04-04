using UnityEngine;
using System;
public class Planet : MonoBehaviour, IInteractable
{
    [SerializeField] private PlanetSO planetData;
    public static event Action<PlanetSO> Interacted;
    public void OnInteract(IInteractor interactor)
    {
        Interacted?.Invoke(planetData);
    }


    public void OnInteractorEnterRange(IInteractor interactor)
    {
        
    }

    public void OnInteractorExitRange(IInteractor interactor)
    {
        
    }
}
