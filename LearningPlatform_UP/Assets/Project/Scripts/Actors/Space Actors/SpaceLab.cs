using UnityEngine;

public class SpaceLab : WorldObject
{
    public override string LabelName => "Space Lab";
    private const string SCENE_NAME = "LabScene";
    public override void OnInteract(IInteractor interactor)
    {
        ExplorationAreaController.Instance.LoadArea(SCENE_NAME, true);
    }
}
