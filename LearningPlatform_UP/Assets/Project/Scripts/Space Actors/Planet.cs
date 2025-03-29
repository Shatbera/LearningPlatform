using System;
using UnityEngine;

public class Planet : MonoBehaviour, IInteractable
{
    [SerializeField] private ObjectInfoSO objectInfo;
    public void OnInteract(IInteractor interactor)
    {
        if(objectInfo != null)
        {
            PopupsController.Instance.OpenPopup(IPopupsController.PopupTag.ObjectInfo, onComplete: SetupPopup);
        }
    }

    private void SetupPopup(Popup popup)
    {
        popup.GetComponent<ObjectInfoPopup>().Setup(objectInfo);
    }

    public void OnInteractorEnterRange(IInteractor interactor)
    {
        
    }

    public void OnInteractorExitRange(IInteractor interactor)
    {
        
    }
}
