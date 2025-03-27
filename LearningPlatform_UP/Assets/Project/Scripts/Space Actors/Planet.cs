using UnityEngine;

public class Planet : MonoBehaviour, IInteractable
{
    public void OnInteract(IInteractor interactor)
    {
        PopupsController.Instance.OpenPopup(IPopupsController.PopupTag.ObjectInfo);
    }

    public void OnInteractorEnterRange(IInteractor interactor)
    {
        
    }

    public void OnInteractorExitRange(IInteractor interactor)
    {
        
    }
}
