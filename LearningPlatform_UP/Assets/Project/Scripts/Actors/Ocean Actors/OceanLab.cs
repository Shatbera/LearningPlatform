using UnityEngine;

public class OceanLab : WorldObject
{
    public override string LabelName => "Laboratory";
    private const string SCENE_NAME = "OceanLabScene";
    public override void OnInteract(IInteractor interactor)
    {
        ExplorationAreaController.Instance.LoadArea(SCENE_NAME, false);
    }
}
