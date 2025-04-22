using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public List<Entry> Entries = new();

    [System.Serializable]
    public class Entry
    {
        public string Key;
        public string TypeName;
        public string Json;
    }

    public Dictionary<string, object> ToDictionary()
    {
        var result = new Dictionary<string, object>();
        foreach (var entry in Entries)
        {
            var type = System.Type.GetType(entry.TypeName);
            var obj = JsonUtility.FromJson(entry.Json, type);
            result[entry.Key] = obj;
        }
        return result;
    }
}
