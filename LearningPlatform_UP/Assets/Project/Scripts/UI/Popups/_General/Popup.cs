using System;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [field: SerializeField] public PopupTag PopupTag;
    [field: SerializeField] public bool CloseWithTint;

    [SerializeField] private PopupVisibilityControllerBase visibilityController;

    public event Action Opened;
    public event Action Closed;

    private void Start()
    {
        if (visibilityController == null)
        {
            Debug.LogError($"Popup {name} has no visibility controller assigned.", this);
            return;
        }

        visibilityController.Initialize(this);
    }

    private void OnDestroy()
    {
        visibilityController?.Dispose(this);
    }

    public void Open()
    {
        PopupsController.Instance.OpenPopup(this);
    }

    public void Close()
    {
        PopupsController.Instance.ClosePopup(this);
    }

    public void SetVisible(bool visible)
    {
        if (visibilityController == null)
        {
            Debug.LogError($"Popup {name} has no visibility controller assigned.", this);
            return;
        }

        visibilityController.SetVisible(this, visible);
    }


    public void NotifyOpened()
    {
        Opened?.Invoke();
        if (AudioManagerGlobal.Instance != null)
        {
            AudioManagerGlobal.Instance.PlayOneShot("openPaper");
        }
    }

    public void NotifyClosed()
    {
        Closed?.Invoke();
    }
}
