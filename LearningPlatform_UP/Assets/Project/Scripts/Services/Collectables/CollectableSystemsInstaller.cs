using UnityEngine;
public class CollectableSystemsInstaller : MonoBehaviour
{
    [SerializeField] private ItemsContainerBaseSO<ResearchSampleSO> _researchSamples;
    [SerializeField] private ResearchSampleSystemServiceSO _researchSystemService;

    private void Awake()
    {
        InstallServices();
    }
    public void InstallServices()
    {
        _researchSystemService.InstallService(new CollectableItemSystem<ResearchSampleSO>(_researchSamples.Items));
    }
}
