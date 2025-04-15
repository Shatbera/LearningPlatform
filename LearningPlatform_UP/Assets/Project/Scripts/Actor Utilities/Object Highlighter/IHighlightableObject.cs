using System;
using UnityEngine;
public interface IHighlightableObject
{
    public string LabelName { get; }
    public SpriteRenderer Renderer { get; }
    public static event Action<IHighlightableObject, bool, bool> Highlighted;
    public static void Highlight(IHighlightableObject highlightable, bool highlight, bool zoomCamera = false)
    {
        Highlighted?.Invoke(highlightable, highlight, zoomCamera);
    }
    public void OnHighlight(bool highlight);
}
