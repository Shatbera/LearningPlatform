using UnityEngine;
using UnityEngine.Localization;

public class Planet : WorldObject
{
    [SerializeField] private PlanetSO planetData;
    [SerializeField] private GameObject lockedVisual;

    public override LocalizedString LocalizedLabelName => planetData.LocalizedObjectName;
    public PlanetSO Data => planetData;

    private PlanetState _state;

    //public static event Action<PlanetSO> Interacted;

    private void Awake()
    {
        RefreshLockedVisual();
    }

    public void SetState(PlanetState state)
    {
        if (_state != null)
        {
            _state.LockChanged -= OnLockChanged;
        }

        _state = state;
        if (_state != null)
        {
            _state.LockChanged += OnLockChanged;
        }

        RefreshLockedVisual();
    }

    private void OnDestroy()
    {
        if (_state != null)
        {
            _state.LockChanged -= OnLockChanged;
        }
    }

    public override void OnInteract(IInteractor interactor)
    {
        if (IsLocked())
        {
            return;
        }

        //Interacted?.Invoke(planetData);
        /*PopupsController.Instance.OpenPopup(IPopupsController.PopupTag.ObjectInfo, onComplete: p =>
        {
            p.GetComponent<ObjectInfoPopup>().Setup(planetData);
        });*/
        ExplorationAreaController.Instance.LoadArea(planetData.SceneName, true, () =>
        {
            // PopupsController.Instance.OpenPopup(PopupTag.ObjectInfo, onComplete: window =>
            // {
            //     window.GetComponent<ObjectInfoPopup>().Setup(planetData);
            // });
        });
    }

    private bool IsLocked()
    {
        return _state == null ? planetData.InitiallyLocked : _state.IsLocked;
    }

    private void OnLockChanged(bool isLocked)
    {
        RefreshLockedVisual();
    }

    private void RefreshLockedVisual()
    {
        if (lockedVisual != null)
        {
            lockedVisual.SetActive(IsLocked());
        }
    }
}
