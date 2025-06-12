using UnityEngine;
using UnityEngine.Localization;
public interface IHighlightableSpaceObject : IHighlightableObject
{
    public LocalizedString LocalizedLabelName { get; }
    public SpriteRenderer Renderer { get; }
}
