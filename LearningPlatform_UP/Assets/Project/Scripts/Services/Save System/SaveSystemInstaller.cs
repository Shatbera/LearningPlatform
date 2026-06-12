using UnityEngine;

public class SaveSystemInstaller : ServiceInstaller
{
    [SerializeField] private SaveSystemServiceRefSO _saveSystemServiceRef;

    public override void Install()
    {
        _saveSystemServiceRef.InstallService(new SaveManager());
    }
}
