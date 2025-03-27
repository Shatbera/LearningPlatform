public interface IInteractable
{
    void OnInteractorEnterRange(IInteractor interactor);

    void OnInteractorExitRange(IInteractor interactor);

    void OnInteract(IInteractor interactor);
}
