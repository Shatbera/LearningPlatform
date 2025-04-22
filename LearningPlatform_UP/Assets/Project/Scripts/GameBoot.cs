using UnityEngine;

public class GameBoot : MonoBehaviour
{
    [SerializeField] private ServicesBootstrap _servicesBootstrap;
    [SerializeField] private SaveSystemServiceRefSO _saveSystemServiceRefSO;

    private bool _canSave = false;
    private void Awake()
    {
        _servicesBootstrap.InstallServices();
    }

    private void Start()
    {
        _saveSystemServiceRefSO.Service.Load();
        _saveSystemServiceRefSO.Service.TryRestoreRegistered();
        _canSave = true;
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            if(_canSave && _saveSystemServiceRefSO.Service != null)
            {
                Debug.Log("[GameBoot] Auto-saving on focus lost...");
                _saveSystemServiceRefSO.Service.Save();
            }
        }
    }
}
