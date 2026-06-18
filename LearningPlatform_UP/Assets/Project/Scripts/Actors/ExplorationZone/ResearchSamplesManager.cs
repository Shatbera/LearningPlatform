using UnityEngine;

public class ResearchSamplesManager : MonoBehaviour
{
    [SerializeField] private string Id;
    public string SceneId => string.IsNullOrEmpty(Id) ? gameObject.scene.name : Id;
}
