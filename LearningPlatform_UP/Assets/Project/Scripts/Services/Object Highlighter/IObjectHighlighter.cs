using UnityEngine;

public interface IObjectHighlighter<THighlightable> where THighlightable : IHighlightableObject
{
    public void Highlight(THighlightable highlightable);
    public void Unhighlight(THighlightable highlightable);
}

public interface IHighlightableObject
{
    
}
