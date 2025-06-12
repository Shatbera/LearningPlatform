using UnityEngine;
using UnityEngine.Localization;

public class OceanLab : WorldObject
{
    [SerializeField] private LocalizedString _localizedName;
    public override LocalizedString LocalizedLabelName => _localizedName;
    private const string SCENE_NAME = "OceanLabScene";
    public override void OnInteract(IInteractor interactor)
    {
        ExplorationAreaController.Instance.LoadArea(SCENE_NAME, false);
    }
}
