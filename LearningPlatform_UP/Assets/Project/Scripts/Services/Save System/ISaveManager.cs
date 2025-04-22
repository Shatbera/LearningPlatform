public interface ISaveManager
{
    void Register(ISaveable saveable);
    void Unregister(ISaveable saveable);

    void Save(string slotName = "save");
    void Load(string slotName = "save");

    void TryRestoreRegistered();
}
