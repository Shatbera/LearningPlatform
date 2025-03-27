using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private InteractionButton interactionButton;

    private void Awake()
    {
        interactionButton.SetInteractor(player);
    }
}
