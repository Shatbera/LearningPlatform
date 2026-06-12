using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class MissionPanelUI : MonoBehaviour
{
    [SerializeField] private MissionSystemRefSO _missionSystemRef;
    [SerializeField] private GameObject _container;
    [SerializeField] private LocalizeStringEvent _titleText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private GameObject _progressContainer;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private Button _button;

    private IMissionSystem _missionSystem;

    private void OnEnable()
    {
        Bind();
        _button?.onClick.AddListener(OnClicked);
        Refresh(_missionSystem?.CurrentMission);
    }

    private void Start()
    {
        Bind();
        Refresh(_missionSystem?.CurrentMission);
    }

    private void OnDisable()
    {
        if (_missionSystem != null)
        {
            _missionSystem.CurrentMissionChanged -= Refresh;
            _missionSystem.MissionProgressChanged -= Refresh;
            _missionSystem = null;
        }

        _button?.onClick.RemoveListener(OnClicked);
    }

    private void Bind()
    {
        if (_missionSystem != null || _missionSystemRef == null || _missionSystemRef.Service == null)
        {
            return;
        }

        _missionSystem = _missionSystemRef.Service;
        _missionSystem.CurrentMissionChanged += Refresh;
        _missionSystem.MissionProgressChanged += Refresh;
    }

    private void Refresh(MissionRuntime mission)
    {
        bool hasMission = mission != null;
        if (_container != null)
        {
            _container.SetActive(hasMission);
        }

        if (!hasMission)
        {
            return;
        }

        SetLocalizedString(_titleText, mission.Definition.Title);

        if (_progressText != null)
        {
            bool shouldShowProgress = mission.TargetAmount > 1;
            _progressContainer?.SetActive(shouldShowProgress);

            if (shouldShowProgress)
            {
                _progressText.text = $"{mission.CurrentAmount}/{mission.TargetAmount}";
            }
        }

        if (_iconImage != null)
        {
            _iconImage.sprite = mission.Definition.Icon;
            _iconImage.enabled = mission.Definition.Icon != null;
        }
    }

    private void OnClicked()
    {
        _missionSystem?.RequestCurrentMissionHint();
    }

    private void SetLocalizedString(LocalizeStringEvent localizeStringEvent, LocalizedString value)
    {
        if (localizeStringEvent == null)
        {
            return;
        }

        localizeStringEvent.StringReference = value;
        localizeStringEvent.RefreshString();
    }
}
