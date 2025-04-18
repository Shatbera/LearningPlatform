using UnityEngine;
public interface IHighlightableSpaceObject : IHighlightableObject
{
    public string LabelName { get; }
    public SpriteRenderer Renderer { get; }
}
