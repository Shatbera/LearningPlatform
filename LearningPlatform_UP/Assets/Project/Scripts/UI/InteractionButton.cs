using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject container;

    private IInteractor _interactor;
    private IInteractable _interactable;
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
        _interactable = null;
        SetVisible(false);
        _interactor = interactor;
        interactor.InteractableAdded += OnInteractableAdded;
        interactor.InteractableRemoved += OnInteractableRemoved;
    }

    private void OnClick()
    {
        if(_interactor == null || _interactable == null) return;
        _interactor.Interact(_interactable);
    }

    public void RemoveInteractor()
    {
        if(_interactor != null)
        {
            _interactor.InteractableAdded -= OnInteractableAdded;
            _interactor.InteractableRemoved -= OnInteractableRemoved;
        }
        _interactor = null;
        _interactable = null;
        SetVisible(false);
    } 

    private void OnInteractableAdded(IInteractable interactable)
    {
        _interactable = interactable;
        SetVisible(true);
    }

    private void OnInteractableRemoved(IInteractable interactable)
    {
        if(_interactable == interactable)
        {
            _interactable = null;
        }
        SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        container.SetActive(visible);
    }
}
