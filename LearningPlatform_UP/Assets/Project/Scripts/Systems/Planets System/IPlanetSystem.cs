using System.Collections.Generic;

public interface IPlanetSystem
{
    PlanetState GetState(Planet planet);
    PlanetState GetState(string planetId);
    bool IsLocked(Planet planet);
    bool IsLocked(string planetId);
    bool Lock(Planet planet);
    bool Lock(string planetId);
    bool Unlock(Planet planet);
    bool Unlock(string planetId);
    IEnumerable<PlanetState> GetAll();
}

[System.Serializable]
public class PlanetState
{
    public string Id;
    public bool IsLocked;

    public event System.Action<bool> LockChanged;

    public PlanetState()
    {
    }

    public PlanetState(string id, bool isLocked)
    {
        Id = id;
        IsLocked = isLocked;
    }

    public void SetLocked(bool isLocked)
    {
        if (IsLocked == isLocked)
        {
            return;
        }

        IsLocked = isLocked;
        LockChanged?.Invoke(IsLocked);
    }
}
