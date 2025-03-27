using UnityEngine;

public class Planet : MonoBehaviour, IInteractable
{
    public void OnInteract(IInteractor interactor)
    {
        Debug.Log(name + ": brbr");
    }

    public void OnInteractorEnterRange(IInteractor interactor)
    {
        Debug.Log("hi im " + name);
    }

    public void OnInteractorExitRange(IInteractor interactor)
    {
        Debug.Log(name+": bye");
    }
}
