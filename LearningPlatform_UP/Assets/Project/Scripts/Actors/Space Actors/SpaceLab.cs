using UnityEngine;
using UnityEngine.Localization;

public class SpaceLab : WorldObject
{
    [SerializeField] private LocalizedString _localizedName;
    public override LocalizedString LocalizedLabelName => _localizedName;
    private const string SCENE_NAME = "LabScene";
    public override void OnInteract(IInteractor interactor)
    {
        ExplorationAreaController.Instance.LoadArea(SCENE_NAME, true, SceneVisibilityMode.Lab);
    }
}
