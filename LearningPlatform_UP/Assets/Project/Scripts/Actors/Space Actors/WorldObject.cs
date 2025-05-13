using UnityEngine;

public abstract class WorldObject : MonoBehaviour, IInteractable, IHighlightableSpaceObject
{
    public abstract string LabelName { get; }
    [SerializeField] private SpriteRenderer _renderer;
    public SpriteRenderer Renderer => _renderer;
    [SerializeField] private SpaceObjectHighlighterServiceRefSO _highlighterServiceRef;

    public abstract void OnInteract(IInteractor interactor);

    public virtual void OnInteractorEnterRange(IInteractor interactor)
    {
        _highlighterServiceRef.Service.Highlight(this);
    }

    public virtual void OnInteractorExitRange(IInteractor interactor)
    {
        _highlighterServiceRef.Service.Unhighlight(this);
    }
}
