using System;
using UnityEngine;
using UnityEngine.Localization;

public class OceanCreature : WorldObject
{
    [SerializeField] private OceanCreatureSO _creatureData;
    public override LocalizedString LocalizedLabelName => _creatureData.LocalizedObjectName;

    public override void OnInteract(IInteractor interactor)
    {
        PopupsController.Instance.OpenPopup(PopupTag.ObjectInfo, OnPopupOpen);
    }

    private void OnPopupOpen(Popup obj)
    {
        (obj).GetComponent<ObjectInfoPopup>().Setup(_creatureData);
    }
}
