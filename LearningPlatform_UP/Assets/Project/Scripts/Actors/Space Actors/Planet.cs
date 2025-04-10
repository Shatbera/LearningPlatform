using UnityEngine;
using System;
public class Planet : MonoBehaviour, IInteractable, IHighlightableObject
{
    [SerializeField] private PlanetSO planetData;
    [SerializeField] private SpriteRenderer _renderer;
    public SpriteRenderer Renderer => _renderer;

    public string LabelName => planetData.ObjectName;

    public static event Action<PlanetSO> Interacted;

    public void OnInteract(IInteractor interactor)
    {
        Interacted?.Invoke(planetData);
        /*PopupsController.Instance.OpenPopup(IPopupsController.PopupTag.ObjectInfo, onComplete: p =>
        {
            p.GetComponent<ObjectInfoPopup>().Setup(planetData);
        });*/
    }


    public void OnInteractorEnterRange(IInteractor interactor)
    {
        IHighlightableObject.Highlight(this, true);        
    }

    public void OnInteractorExitRange(IInteractor interactor)
    {
        IHighlightableObject.Highlight(this, false);
    }

    public void OnHighlight(bool highlight)
    {

    }
}
