using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PopupOpener : MonoBehaviour
{
    [SerializeField] private PopupTag _popupTag;
    private void Open()
    {
        PopupsController.Instance.OpenPopup(_popupTag);
    }

    private void Awake()
    {
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(Open);
        }
    }
}
