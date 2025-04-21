using UnityEngine;

public class ResearchPadInstaller : ServiceInstaller
{
    [SerializeField] private ResearchPad _researchPad;
    [SerializeField] private ResearchPadRefSO _researchPadRef;
    public override void Install()
    {
        _researchPadRef.InstallService(_researchPad);
    }
}
