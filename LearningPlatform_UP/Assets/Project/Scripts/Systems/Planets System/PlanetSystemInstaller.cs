using UnityEngine;

public class PlanetSystemInstaller : ServiceInstaller
{
    [SerializeField] private Planet[] _planets;
    [SerializeField] private PlanetSystemRefSO _planetSystemRef;
    [SerializeField] private SaveSystemServiceRefSO _saveSystemRef;

    public override void Install()
    {
         var planetSystem = new PlanetSystem(_planets);
        _planetSystemRef.InstallService(planetSystem);
        _saveSystemRef.Service.Register(planetSystem);
    }
}
