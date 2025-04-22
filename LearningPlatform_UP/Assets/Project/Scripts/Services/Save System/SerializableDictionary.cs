using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue>
{
    [Serializable]
    public struct KeyValuePair
    {
        public TKey Key;
        public TValue Value;
    }

    [SerializeField]
    private List<KeyValuePair> _entries = new();

    public void FromDictionary(Dictionary<TKey, TValue> dict)
    {
        _entries.Clear();
        foreach (var pair in dict)
        {
            _entries.Add(new KeyValuePair { Key = pair.Key, Value = pair.Value });
        }
    }

    public Dictionary<TKey, TValue> ToDictionary()
    {
        var result = new Dictionary<TKey, TValue>();
        foreach (var entry in _entries)
        {
            if (!result.ContainsKey(entry.Key))
                result[entry.Key] = entry.Value;
        }
        return result;
    }

    public List<KeyValuePair> Entries => _entries;
}
