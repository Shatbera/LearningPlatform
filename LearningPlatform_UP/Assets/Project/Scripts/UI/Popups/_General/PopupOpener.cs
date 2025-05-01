using UnityEngine;

public class PopupOpener : MonoBehaviour
{
    [SerializeField] private PopupTag _popupTag;
    public void Open()
    {
        PopupsController.Instance.OpenPopup(_popupTag);
    }
}
