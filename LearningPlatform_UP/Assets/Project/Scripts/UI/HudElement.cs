using UnityEngine;

public class HudElement : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    public void SetVisible(bool visible)
    {
        _container.SetActive(visible);
    }
}
