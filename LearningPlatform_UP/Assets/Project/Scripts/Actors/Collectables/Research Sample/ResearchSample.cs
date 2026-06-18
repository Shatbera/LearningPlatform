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
    [SerializeField] private GameObject _lockedVisual;
    [SerializeField] private LockedFeedbackShake2D _lockedFeedback;
    public CollectableItemSO ItemSO => _sampleSO;

    public SpriteRenderer Renderer => _renderer;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _animator;
    private string _collectionId;
    private ResearchSampleItemState _state;

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
        BindState();

        if (_animator != null)
        {
            _animator.SetFloat("Offset", Random.Range(0f, 1f));
            _animator.SetInteger("Animation", (int)_animation);
        }
    }

    private void OnDestroy()
    {
        if (_state != null)
        {
            _state.UnlockedChanged -= OnUnlockedChanged;
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

        BindState();
        if (_state == null || !_state.IsUnlocked)
        {
            OnClickWhenLocked();
            return;
        }

        if (_researchSamplesSystem.Service.AddItem(_sampleSO.Id, 1))
        {
            WorldCollectableStateSystem.Current?.MarkCollected(_collectionId);
            Pickup();
        }
    }

    private void OnClickWhenLocked()
    {
        _lockedFeedback?.Play();
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

    private void BindState()
    {
        if (_researchSamplesSystem == null || _researchSamplesSystem.Service == null || _sampleSO == null)
        {
            RefreshLockedVisual();
            return;
        }

        CollectableItemEntry<ResearchSampleSO, ResearchSampleItemState> sampleEntry = _researchSamplesSystem.Service.GetItem(_sampleSO.Id);
        ResearchSampleItemState state = sampleEntry == null ? null : sampleEntry.State;
        if (_state == state)
        {
            RefreshLockedVisual();
            return;
        }

        if (_state != null)
        {
            _state.UnlockedChanged -= OnUnlockedChanged;
        }

        _state = state;
        if (_state != null)
        {
            _state.UnlockedChanged += OnUnlockedChanged;
        }

        RefreshLockedVisual();
    }

    private void OnUnlockedChanged(bool isUnlocked)
    {
        RefreshLockedVisual();
    }

    private void RefreshLockedVisual()
    {
        if (_lockedVisual != null)
        {
            _lockedVisual.SetActive(_state == null || !_state.IsUnlocked);
        }

        if (_animator != null)
        {
            _animator.enabled = _state != null && _state.IsUnlocked;
        }
    }

}
