using System;
using UnityEngine;

public class Player : MonoBehaviour, IInteractor
{
    public event Action<IInteractable> InteractableAdded;
    public event Action<IInteractable> InteractableRemoved;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            OnInteractableEnterRange(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            OnInteractableExitRange(interactable);
        }
    }


    public void Interact(IInteractable interactable)
    {
        interactable.OnInteract(this);
    }

    public void OnInteractableEnterRange(IInteractable interactable)
    {
        interactable.OnInteractorEnterRange(this);
        InteractableAdded?.Invoke(interactable);
    }

    public void OnInteractableExitRange(IInteractable interactable)
    {
        interactable.OnInteractorExitRange(this);
        InteractableRemoved?.Invoke(interactable);
    }
}
