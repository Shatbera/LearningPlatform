using UnityEngine;

public class ResearchSamplesManager : MonoBehaviour, ISaveable
{
    [SerializeField] private string Id;
    public string SaveKey => $"SAMPLES_SCENE_{Id}";

    [System.Serializable]
    public class SaveData
    {
        //TODO
    }
    public object CaptureState()
    {
        var saveData = new SaveData();
        
        return saveData;
    }

    public void RestoreState(object data)
    {
        if (data is not SaveData saveData) return;
    }
}
