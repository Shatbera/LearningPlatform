using UnityEngine;

public abstract class SpaceObject : MonoBehaviour, IInteractable, IHighlightableObject
{
    public abstract string LabelName { get; }
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private bool _zoomCameraOnHighlight;
    public SpriteRenderer Renderer => _renderer;

    public abstract void OnHighlight(bool highlight);

    public abstract void OnInteract(IInteractor interactor);

    public virtual void OnInteractorEnterRange(IInteractor interactor)
    {
        IHighlightableObject.Highlight(this, true, _zoomCameraOnHighlight);
    }

    public virtual void OnInteractorExitRange(IInteractor interactor)
    {
        IHighlightableObject.Highlight(this, false, _zoomCameraOnHighlight);
    }
}
