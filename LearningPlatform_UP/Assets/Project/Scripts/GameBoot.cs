using System.Collections.Generic;
using UnityEngine;

public class GameBoot : MonoBehaviour
{
    public enum World { Space, Ocean }

    [SerializeField] private World _world;
    [SerializeField] private ServicesBootstrap _servicesBootstrap;
    [SerializeField] private SaveSystemServiceRefSO _saveSystemServiceRefSO;

    private bool _canSave = false;

    private static readonly Dictionary<World, string> SaveKeysDict = new()
    {
        { World.Space, "spaceExplorationSave" },
        { World.Ocean, "oceanMysterySave" },
    };

    private string SaveKey => SaveKeysDict[_world];

    private void Awake()
    {
        _servicesBootstrap.InstallServices();
        _saveSystemServiceRefSO.Service.Load(SaveKey);
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
                _saveSystemServiceRefSO.Service.Save(SaveKey);
            }
        }
    }

    private void OnDestroy(){
        if(_canSave){
            Debug.Log("saving on destroy");
            _saveSystemServiceRefSO.Service.Save(SaveKey);
        }
    }
}
