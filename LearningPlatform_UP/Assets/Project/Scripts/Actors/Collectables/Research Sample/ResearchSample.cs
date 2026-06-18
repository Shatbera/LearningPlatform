using UnityEngine;

public class ResearchSample : ClickableObject, IPickableItem
{
    [SerializeField] private ResearchSampleSystemServiceRefSO _researchSamplesSystem;
    [SerializeField] private ItemPickupAnimatorRefSO _itemPickupAnimatorRef;
    private enum AnimationType
    {
        None,
        Floating, 
        Shaking,
        Pulsing,
    }

    [SerializeField] private ResearchSampleSO _sampleSO;
    [SerializeField] private string _instanceId;
    [SerializeField] private AnimationType _animation;
    public CollectableItemSO ItemSO => _sampleSO;

    public SpriteRenderer Renderer => _renderer;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _animator;
    private string _collectionId;

    private void Awake()
    {
        _collectionId = BuildCollectionId();

        if (IsAlreadyCollected())
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (_animator != null)
        {
            _animator.SetFloat("Offset", Random.Range(0f, 1f));
            _animator.SetInteger("Animation", (int)_animation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TryCollect();
        }
    }

    public override void OnClick()
    {
        TryCollect();
    }

    private void TryCollect()
    {
        if (_researchSamplesSystem == null || _researchSamplesSystem.Service == null || _sampleSO == null)
        {
            return;
        }

        if (IsAlreadyCollected())
        {
            return;
        }

        if (_researchSamplesSystem.Service.AddItem(_sampleSO.Id, 1))
        {
            WorldCollectableStateSystem.Current?.MarkCollected(_collectionId);
            Pickup();
        }
    }

    private void Pickup(){
        AudioManagerGlobal.Instance.PlayOneShot("itemPickup");
        AudioManagerGlobal.Instance.PlayOneShot("rewardLight");
        ParticlesManager.Instance.SpawnParticle("itemPickup", transform.position);
        _itemPickupAnimatorRef.Service.AnimatePickup(this);
            Destroy(gameObject);
    }

    private void OnValidate()
    {
        if(_sampleSO != null && _renderer != null)
        {
            _renderer.sprite = _sampleSO.Sprite;
        }
    }

    private string BuildCollectionId()
    {
        if (!string.IsNullOrEmpty(_instanceId))
        {
            return _instanceId;
        }

        return _sampleSO == null ? string.Empty : _sampleSO.Id;
    }

    private bool IsAlreadyCollected()
    {
        return WorldCollectableStateSystem.Current != null
            && WorldCollectableStateSystem.Current.IsCollected(_collectionId);
    }

}
