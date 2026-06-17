using System.Collections.Generic;

public class PlanetSystem : IPlanetSystem, ISaveable
{
    private readonly Dictionary<string, PlanetState> _planetStates = new();
    private readonly Dictionary<string, Planet> _planets = new();

    public PlanetSystem(IEnumerable<Planet> planets)
    {
        if (planets == null)
        {
            return;
        }

        foreach (Planet planet in planets)
        {
            InitializePlanet(planet);
        }
    }

    public string SaveKey => "planetSystem";

    public PlanetState GetState(Planet planet)
    {
        return planet == null || planet.Data == null ? null : GetState(planet.Data.SaveId);
    }

    public PlanetState GetState(string planetId)
    {
        return _planetStates.TryGetValue(planetId, out PlanetState state) ? state : null;
    }

    public bool IsLocked(Planet planet)
    {
        return planet != null && planet.Data != null && IsLocked(planet.Data.SaveId);
    }

    public bool IsLocked(string planetId)
    {
        PlanetState state = GetState(planetId);
        return state != null && state.IsLocked;
    }

    public bool Lock(Planet planet)
    {
        return planet != null && planet.Data != null && Lock(planet.Data.SaveId);
    }

    public bool Lock(string planetId)
    {
        return SetLocked(planetId, true);
    }

    public bool Unlock(Planet planet)
    {
        return planet != null && planet.Data != null && Unlock(planet.Data.SaveId);
    }

    public bool Unlock(string planetId)
    {
        return SetLocked(planetId, false);
    }

    public IEnumerable<PlanetState> GetAll()
    {
        return _planetStates.Values;
    }

    public object CaptureState()
    {
        var saveData = new PlanetSystemSaveData();
        foreach (PlanetState state in _planetStates.Values)
        {
            saveData.Planets.Add(state);
        }

        return saveData;
    }

    public void RestoreState(object data)
    {
        if (data is not PlanetSystemSaveData saveData)
        {
            return;
        }

        foreach (PlanetState state in saveData.Planets)
        {
            if (string.IsNullOrEmpty(state.Id))
            {
                continue;
            }

            if (_planetStates.TryGetValue(state.Id, out PlanetState existingState))
            {
                existingState.IsLocked = state.IsLocked;
            }
            else
            {
                _planetStates[state.Id] = state;
                if (_planets.TryGetValue(state.Id, out Planet planet))
                {
                    planet.SetState(state);
                }
            }
        }
    }

    private void InitializePlanet(Planet planet)
    {
        if (planet == null || planet.Data == null)
        {
            return;
        }

        string planetId = planet.Data.SaveId;
        _planets[planetId] = planet;
        planet.SetState(GetOrCreateState(planet));
    }

    private PlanetState GetOrCreateState(Planet planet)
    {
        string planetId = planet.Data.SaveId;
        if (!_planetStates.TryGetValue(planetId, out PlanetState state))
        {
            state = new PlanetState(planetId, planet.Data.InitiallyLocked);
            _planetStates[planetId] = state;
        }

        return state;
    }

    private bool SetLocked(string planetId, bool isLocked)
    {
        if (string.IsNullOrEmpty(planetId) || !_planetStates.TryGetValue(planetId, out PlanetState state))
        {
            return false;
        }

        state.IsLocked = isLocked;
        return true;
    }
}

[System.Serializable]
public class PlanetSystemSaveData
{
    public List<PlanetState> Planets = new();
}
