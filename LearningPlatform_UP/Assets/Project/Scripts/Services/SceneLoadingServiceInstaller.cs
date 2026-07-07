using UnityEngine;

public class SceneLoadingServiceInstaller : ServiceInstaller
{
    [SerializeField] private SceneLoadingServiceRefSO _sceneLoadingServiceRef;

    public override void Install()
    {
        if (_sceneLoadingServiceRef == null)
        {
            Debug.LogError($"{nameof(SceneLoadingServiceInstaller)} has no scene loading service ref assigned.", this);
            return;
        }

        _sceneLoadingServiceRef.InstallService(new SceneLoadingService());
    }
}
