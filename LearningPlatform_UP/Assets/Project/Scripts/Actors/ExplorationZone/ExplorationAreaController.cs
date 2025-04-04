using UnityEngine;
using UnityEngine.UI;

public class ExplorationAreaController : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private Button _exitButton;

    private ExplorableObjectSO _objectData;

    protected void Awake()
    {
        _exitButton.onClick.AddListener(Exit);
        SetVisible(false);
    }

    private void OnEnable()
    {
        Planet.Interacted += LoadArea;
    }

    private void OnDisable()
    {
        Planet.Interacted -= LoadArea;
    }
    public void LoadArea(ExplorableObjectSO objectData)
    {
        _objectData= objectData;
        _background.sprite = _objectData.BackgroundSprite;
        SetVisible(true);

        PopupsController.Instance.OpenPopup(IPopupsController.PopupTag.ObjectInfo, popup =>
        {
            popup.GetComponent<ObjectInfoPopup>().Setup(_objectData);
        });

        transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);        
    }


    private void Exit()
    {
        SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        _exitButton.gameObject.SetActive(visible);
        _container.SetActive(visible);
    }
}
