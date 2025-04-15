using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExplorationAreaController : Singleton<ExplorationAreaController>
{
    [SerializeField] private CinemachineCamera _objectCamera;

    [SerializeField] private GameObject[] _mainSceneObjects;
    [SerializeField] private Button _exitButton;

    [SerializeField] private Camera _mainCamera;

    [SerializeField] private ScreenFade _screenFade;


    private const float ZOOM_DURATION = 0.75f;
    private const float FADE_DURATION = 0.3f;
    private const float FADE_DELAY = 0.1f;

    private string _loadedAreaSceneName;
    protected override void Awake()
    {
        base.Awake();
        _exitButton.onClick.AddListener(Exit);
    }

    public void LoadArea(string sceneName, bool zoomCamera, System.Action onComplete = null)
    {
        StartCoroutine(LoadAreaCoroutine(sceneName, zoomCamera, onComplete));
    }

    private IEnumerator LoadAreaCoroutine(string sceneName, bool zoomCamera, System.Action onComlete)
    {
        transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);


        if (zoomCamera)
        {
            float startOrtho = _objectCamera.Lens.OrthographicSize;
            bool fading = false;

            for (float t = 0; t < ZOOM_DURATION; t += Time.deltaTime)
            {
                float progress = t / ZOOM_DURATION;
                _objectCamera.Lens.OrthographicSize = Mathf.Lerp(startOrtho, 0.1f, progress);
                if (ZOOM_DURATION - t <= FADE_DURATION && !fading)
                {
                    _screenFade.Fade(true, FADE_DURATION);
                    fading = true;
                }
                yield return null;
            }

            _objectCamera.Lens.OrthographicSize = startOrtho;
        }
        else
        {
            _screenFade.Fade(true, FADE_DURATION);
        }

        SwitchScene(true, sceneName); 

        yield return new WaitForSeconds(FADE_DELAY);
        _screenFade.Fade(false, FADE_DURATION);

        onComlete?.Invoke();
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
