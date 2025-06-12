using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject container;

    private IInteractor _interactor;
    private List<IInteractable> _interactables = new();
    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public void SetInteractor(IInteractor interactor)
    {
        if(_interactor != null)
        {
            _interactor.InteractableAdded -= OnInteractableAdded;
            _interactor.InteractableRemoved -= OnInteractableRemoved;
        }
        _interactables.Clear();
        SetVisible(false);
        _interactor = interactor;
        interactor.InteractableAdded += OnInteractableAdded;
        interactor.InteractableRemoved += OnInteractableRemoved;
    }

    private void OnClick()
    {
        if(_interactor == null || _interactables.Count == 0) return;
        _interactor.Interact(_interactables[0]);
    }

    public void RemoveInteractor()
    {
        if(_interactor != null)
        {
            _interactor.InteractableAdded -= OnInteractableAdded;
            _interactor.InteractableRemoved -= OnInteractableRemoved;
        }
        _interactor = null;
        _interactables.Clear();
        SetVisible(false);
    } 

    private void OnInteractableAdded(IInteractable interactable)
    {
        _interactables.Add(interactable);
        SetVisible(true);
    }

    private void OnInteractableRemoved(IInteractable interactable)
    {
        if (_interactables.Contains(interactable))
        {
            _interactables.Remove(interactable);
        }
        SetVisible(_interactables.Count > 0);
    }

    private void SetVisible(bool visible)
    {
        container.SetActive(visible);
    }
}
