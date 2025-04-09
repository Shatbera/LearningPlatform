using System;
using UnityEngine;
public interface IHighlightableObject
{
    public string LabelName { get; }
    public SpriteRenderer Renderer { get; }
    public static event Action<IHighlightableObject, bool> Highlighted;
    public static void Highlight(IHighlightableObject highlightable, bool highlight)
    {
        Highlighted?.Invoke(highlightable, highlight);
    }
    public void OnHighlight(bool highlight);
}
