using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ExplorationAreaController : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private Button _exitButton;

    [SerializeField] private CinemachineCamera _areaCamera;
    [SerializeField] private CinemachineCamera _objectCamera;

    [SerializeField] private ScreenFade _screenFade;

    private ExplorableObjectSO _objectData;

    private const float ZOOM_DURATION = 0.75f;
    private const float FADE_DURATION = 0.3f;
    private const float FADE_DELAY = 0.1f;

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
        StartCoroutine(LoadAreaCoroutine(objectData));
    }

    private IEnumerator LoadAreaCoroutine(ExplorableObjectSO objectData)
    {
        _objectData = objectData;
        _background.sprite = _objectData.BackgroundSprite;
        transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        _areaCamera.enabled = false;


        float startOrtho = _objectCamera.Lens.OrthographicSize;
        bool fading = false;

        for (float t = 0; t < ZOOM_DURATION; t += Time.deltaTime)
        {
            float progress = t / ZOOM_DURATION;
            _objectCamera.Lens.OrthographicSize = Mathf.Lerp(startOrtho, 0.1f, progress);
            if(ZOOM_DURATION - t <= FADE_DURATION && !fading)
            {
                _screenFade.Fade(true, FADE_DURATION);
                fading = true;
            }
            yield return null;
        }

        SetVisible(true);
        _areaCamera.enabled = true;
        _objectCamera.Lens.OrthographicSize = startOrtho;

        yield return new WaitForSeconds(FADE_DELAY);
        _screenFade.Fade(false, FADE_DURATION);

        PopupsController.Instance.OpenPopup(IPopupsController.PopupTag.ObjectInfo, onComplete: window =>
        {
            window.GetComponent<ObjectInfoPopup>().Setup(_objectData);
        });
    }
    private void Exit()
    {
        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        _screenFade.Fade(true, FADE_DURATION);
        yield return new WaitForSeconds(FADE_DURATION);
        SetVisible(false);
        _areaCamera.enabled = false;
        yield return new WaitForSeconds(FADE_DELAY);
        _screenFade.Fade(false, FADE_DURATION);
    }

    private void SetVisible(bool visible)
    {
        _exitButton.gameObject.SetActive(visible);
        _container.SetActive(visible);
    }
}
