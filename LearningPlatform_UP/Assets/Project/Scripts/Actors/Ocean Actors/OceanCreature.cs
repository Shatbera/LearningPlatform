using System;
using UnityEngine;

public class OceanCreature : WorldObject, IInteractable
{
    [SerializeField] private OceanCreatureSO _creatureData;
    public override string LabelName => _creatureData.ObjectName;

    public override void OnInteract(IInteractor interactor)
    {
        PopupsController.Instance.OpenPopup(PopupTag.ObjectInfo, OnPopupOpen);
    }

    private void OnPopupOpen(Popup obj)
    {
        (obj).GetComponent<ObjectInfoPopup>().Setup(_creatureData);
    }
}
