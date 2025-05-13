using UnityEngine;

public class OceanMysteryInstaller : ServiceInstaller
{
    [SerializeField] private SpaceObjectHighlighterServiceRefSO _highlighterServiceRef;
    [SerializeField] private SpaceObjectHighlighter _highlighter;
    public override void Install()
    {
        _highlighterServiceRef.InstallService(_highlighter);
    }
}
