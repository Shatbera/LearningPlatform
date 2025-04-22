using UnityEngine;

public class ServicesBootstrap : MonoBehaviour
{
    public void InstallServices()
    {
        foreach(var service in GetComponentsInChildren<ServiceInstaller>())
        {
            service.Install();
        }
    }
}
