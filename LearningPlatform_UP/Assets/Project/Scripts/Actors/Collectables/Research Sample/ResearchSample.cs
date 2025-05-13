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
    [SerializeField] private AnimationType _animation;
    public CollectableItemSO ItemSO => _sampleSO;

    public SpriteRenderer Renderer => _renderer;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _animator;
    private void Start()
    {
        _animator.SetFloat("Offset", Random.Range(0f, 1f));
        _animator.SetInteger("Animation", (int)_animation);
    }
    public override void OnClick()
    {
        if(_researchSamplesSystem.Service.AddItem(_sampleSO.Id, 1))
        {
            _itemPickupAnimatorRef.Service.AnimatePickup(this);
            Destroy(gameObject);
        }
    }


    private void OnValidate()
    {
        if(_sampleSO != null)
        {
            _renderer.sprite = _sampleSO.Sprite;
        }
    }

}
