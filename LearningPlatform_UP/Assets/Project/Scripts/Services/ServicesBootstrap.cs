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
        foreach (var service in GetComponentsInChildren<ServiceInstaller>())
        {
            service.Install();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            string folderPath = System.IO.Path.Combine(desktopPath, "Learning Platform Screenshots");
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
            int screenshotIndex = PlayerPrefs.GetInt("screenshotIndex", 0) + 1;
            string filePath = System.IO.Path.Combine(folderPath, $"screenshot_{screenshotIndex}.png");
            PlayerPrefs.SetInt("screenshotIndex", screenshotIndex);
            ScreenCapture.CaptureScreenshot(filePath);
        }
    }
}
