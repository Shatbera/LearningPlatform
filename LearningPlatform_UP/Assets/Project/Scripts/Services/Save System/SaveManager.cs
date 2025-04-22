using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : ISaveManager
{
    private readonly List<ISaveable> _saveables = new();
    private Dictionary<string, object> _loadedState = new();
    private bool _isLoaded = false;

    public void Register(ISaveable saveable)
    {
        if (!_saveables.Contains(saveable))
            _saveables.Add(saveable);

        // Restore if already loaded
        if (_isLoaded && _loadedState.TryGetValue(saveable.SaveKey, out var state))
        {
            saveable.RestoreState(state);
        }
    }

    public void Unregister(ISaveable saveable)
    {
        _saveables.Remove(saveable);
    }

    public void Save(string slotName = "save")
    {
        var file = new SaveFile();

        foreach (var s in _saveables)
        {
            object state = s.CaptureState();
            if (state == null) continue;

            file.Entries.Add(new SaveFile.Entry
            {
                Key = s.SaveKey,
                TypeName = state.GetType().AssemblyQualifiedName,
                Json = JsonUtility.ToJson(state)
            });
        }

        var json = JsonUtility.ToJson(file, prettyPrint: true);
        File.WriteAllText(GetPath(slotName), json);
        Debug.Log($"[SaveManager] Saved to: {GetPath(slotName)}");
    }

    public void Load(string slotName = "save")
    {
        string path = GetPath(slotName);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"[SaveManager] No save file found at {path}");
            return;
        }

        string json = File.ReadAllText(path);
        var file = JsonUtility.FromJson<SaveFile>(json);
        _loadedState = file.ToDictionary();
        _isLoaded = true;

        Debug.Log($"[SaveManager] Loaded from: {path}");
    }

    public void TryRestoreRegistered()
    {
        if (!_isLoaded || _loadedState == null) return;

        foreach (var s in _saveables)
        {
            if (_loadedState.TryGetValue(s.SaveKey, out var state))
            {
                s.RestoreState(state);
            }
        }
    }

    private string GetPath(string slotName)
    {
        return Path.Combine(Application.persistentDataPath, $"{slotName}.json");
    }
}
