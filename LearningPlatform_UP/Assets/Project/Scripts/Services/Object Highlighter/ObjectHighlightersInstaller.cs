using UnityEngine;

public class ObjectHighlightersInstaller : ServiceInstaller
{
    [SerializeField] private SpaceObjectHighlighterServiceRefSO _spaceObjectHighlighterServiceRef;
    [SerializeField] private SpaceObjectHighlighterServiceRefSO _spaceObjectCameraHighlighterServiceRef;

    [SerializeField] private SpaceObjectHighlighter _spaceObjectHighlighter;
    [SerializeField] private SpaceObjectCameraHighlighter _spaceObjectCameraHighlighter;

    public override void Install()
    {
        _spaceObjectHighlighterServiceRef.InstallService(_spaceObjectHighlighter);
        _spaceObjectCameraHighlighterServiceRef.InstallService(_spaceObjectCameraHighlighter);
    }
}
