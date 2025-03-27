using UnityEngine;

public class Player : MonoBehaviour, IInteractor
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            OnInteractableEnterRange(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            OnInteractableExitRange(interactable);
        }
    }


    public void Interact(IInteractable interactable)
    {
        interactable.OnInteract(this);
        Debug.Log("player interact with " + (interactable as MonoBehaviour).name);
    }

    public void OnInteractableEnterRange(IInteractable interactable)
    {
        interactable.OnInteractorEnterRange(this);
        Debug.Log("player entered " + (interactable as MonoBehaviour).name);
    }

    public void OnInteractableExitRange(IInteractable interactable)
    {
        interactable.OnInteractorExitRange(this);
        Debug.Log("player exit " + (interactable as MonoBehaviour).name);
    }
}
