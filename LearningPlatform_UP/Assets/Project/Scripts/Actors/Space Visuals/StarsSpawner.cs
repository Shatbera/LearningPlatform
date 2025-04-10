using UnityEngine;
using System.Collections;

public class StarsSpawner : MonoBehaviour
{
    [SerializeField] private ComponentPool<FlickeringStar> _starsPool;
    [SerializeField] private float minSpawnInterval = 0.2f;
    [SerializeField] private float maxSpawnInterval = 1.0f;

    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnStarAtRandomPosition();
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnStarAtRandomPosition()
    {
        Vector2 screenPos = new Vector2(
            Random.Range(0f, Screen.width),
            Random.Range(0f, Screen.height)
        );

        Vector3 worldPos = _mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));

        FlickeringStar star = _starsPool.Get();
        star.Initialize(_starsPool);
        star.transform.position = worldPos;
        star.gameObject.SetActive(true);
    }
}
