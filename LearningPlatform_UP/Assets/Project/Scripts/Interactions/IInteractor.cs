using System;

public interface IInteractor
{
    public event Action<IInteractable> InteractableAdded, InteractableRemoved;
    void OnInteractableEnterRange(IInteractable interactable);

    void OnInteractableExitRange(IInteractable interactable);

    void Interact(IInteractable interactable);
}
