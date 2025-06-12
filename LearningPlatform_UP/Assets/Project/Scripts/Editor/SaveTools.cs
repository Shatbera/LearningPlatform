using UnityEditor;
using UnityEngine;
using System.IO;

public class SaveTools
{
    [MenuItem("Tools/Clear All Save Files")]
    public static void ClearAllSaveFiles()
    {
        string path = Application.persistentDataPath;

        if (!Directory.Exists(path))
        {
            Debug.LogWarning($"[SaveTools] No save directory found at: {path}");
            return;
        }

        string[] files = Directory.GetFiles(path, "*.json");

        int deletedCount = 0;
        foreach (var file in files)
        {
            File.Delete(file);
            deletedCount++;
        }

        if (deletedCount > 0)
            Debug.Log($"[SaveTools] Deleted {deletedCount} save file(s) from: {path}");
        else
            Debug.Log("[SaveTools] No save files found to delete.");
    }
}
