using UnityEngine;

public class ServicesBootstrap : MonoBehaviour
{
    [SerializeField] private bool InstallOnAwake;

    private void Awake()
    {
        if (InstallOnAwake)
        {
            InstallServices();
        }
    }
    public void InstallServices()
    {
        foreach(var service in GetComponentsInChildren<ServiceInstaller>())
        {
            service.Install();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ScreenCapture.CaptureScreenshot("screenshot.png");
        }
    }
}
