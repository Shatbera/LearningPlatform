using UnityEngine;

public class ServicesBootstrap : MonoBehaviour
{
    private void Awake()
    {
        InstallServices();
    }
    private void InstallServices()
    {
        foreach(var service in GetComponentsInChildren<ServiceInstaller>())
        {
            service.Install();
        }
    }
}
