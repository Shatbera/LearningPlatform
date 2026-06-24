using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantProgressBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField, Range(0f, 1f)] private float _initialProgress;

    public float Progress { get; private set; }
    public bool IsComplete => Progress >= 1f;

    private void Awake()
    {
        SetProgress01(_initialProgress);
    }

    public void SetProgress01(float progress)
    {
        Progress = Mathf.Clamp01(progress);

        if (_fillImage != null)
        {
            _fillImage.fillAmount = Progress;
        }

        if (_progressText != null)
        {
            _progressText.text = Mathf.RoundToInt(Progress * 100f).ToString() + "%";
        }
    }

    public void AddProgress(float amount)
    {
        SetProgress01(Progress + amount);
    }
}
