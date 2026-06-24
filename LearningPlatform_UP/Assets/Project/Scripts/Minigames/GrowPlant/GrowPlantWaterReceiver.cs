using UnityEngine;

public class GrowPlantWaterReceiver : MonoBehaviour
{
    [SerializeField] private PlantProgressBar _progressBar;
    [SerializeField] private float _progressPerCollision = 0.005f;

    private float _progress;

    public bool IsComplete => _progress >= 1f; 

    private void Awake()
    {
        UpdateProgressBar();
    }

    public void ReceiveWater(int collisionCount)
    {
        if (IsComplete)
        {
            return;
        }

        AddProgress(_progressPerCollision * Mathf.Max(1, collisionCount));
    }

    private void AddProgress(float amount)
    {
        _progress = Mathf.Clamp01(_progress + amount);
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        if (_progressBar != null)
        {
            _progressBar.SetProgress01(_progress);
        }
    }
}
