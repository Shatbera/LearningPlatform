using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string _loadSceneName;
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(_loadSceneName);
    }
}
