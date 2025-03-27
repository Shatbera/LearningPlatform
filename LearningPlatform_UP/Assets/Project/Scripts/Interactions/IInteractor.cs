public interface IInteractor
{
    void OnInteractableEnterRange(IInteractable interactable);

    void OnInteractableExitRange(IInteractable interactable);

    void Interact(IInteractable interactable);
}
