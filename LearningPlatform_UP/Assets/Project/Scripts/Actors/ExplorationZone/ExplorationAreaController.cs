using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExplorationAreaController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _objectCamera;

    [SerializeField] private GameObject[] _mainSceneObjects;
    [SerializeField] private Button _exitButton;

    [SerializeField] private Camera _mainCamera;

    [SerializeField] private ScreenFade _screenFade;

    private ExplorableObjectSO _objectData;

    private const float ZOOM_DURATION = 0.75f;
    private const float FADE_DURATION = 0.3f;
    private const float FADE_DELAY = 0.1f;

    private string _loadedAreaSceneName;
    private void Awake()
    {
        _exitButton.onClick.AddListener(Exit);
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
        transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);


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

        _objectCamera.Lens.OrthographicSize = startOrtho;

        SwitchScene(true, objectData.SceneName);

        yield return new WaitForSeconds(FADE_DELAY);
        _screenFade.Fade(false, FADE_DURATION);

        PopupsController.Instance.OpenPopup(IPopupsController.PopupTag.ObjectInfo, onComplete: window =>
        {
            window.GetComponent<ObjectInfoPopup>().Setup(_objectData);
        });
    }

    public void SwitchScene(bool exploration, string sceneName = null)
    {
        _exitButton.gameObject.SetActive(exploration);
        foreach(var obj in _mainSceneObjects)
        {
            obj.SetActive(!exploration);
        }
        if (exploration)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            _loadedAreaSceneName = sceneName;
        }
        else
        {
            SceneManager.UnloadSceneAsync(_loadedAreaSceneName);
        }
        //_mainCamera.enabled = !exploration;
    }
    public void Exit()
    {
        StartCoroutine(ExitCoroutine());
    }

    private IEnumerator ExitCoroutine()
    {
        _screenFade.Fade(true, FADE_DURATION);
        yield return new WaitForSeconds(FADE_DURATION);
        SwitchScene(false);
        yield return new WaitForSeconds(FADE_DELAY);
        _screenFade.Fade(false, FADE_DURATION);
    }
}
