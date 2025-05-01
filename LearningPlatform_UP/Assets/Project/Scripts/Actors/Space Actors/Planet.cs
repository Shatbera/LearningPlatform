using UnityEngine;
using System;
public class Planet : SpaceObject
{
    [SerializeField] private PlanetSO planetData;

    public override string LabelName => planetData.ObjectName;

    //public static event Action<PlanetSO> Interacted;

    public override void OnInteract(IInteractor interactor)
    {
        //Interacted?.Invoke(planetData);
        /*PopupsController.Instance.OpenPopup(IPopupsController.PopupTag.ObjectInfo, onComplete: p =>
        {
            p.GetComponent<ObjectInfoPopup>().Setup(planetData);
        });*/
        ExplorationAreaController.Instance.LoadArea(planetData.SceneName, true, () =>
        {
            PopupsController.Instance.OpenPopup(PopupTag.ObjectInfo, onComplete: window =>
            {
                window.GetComponent<ObjectInfoPopup>().Setup(planetData);
            });
        });
    }
}
