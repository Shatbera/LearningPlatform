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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("PLayer entered");
            if (_researchSamplesSystem.Service.AddItem(_sampleSO.Id, 1))
            {
                Pickup();
            }
        }
    }

    public override void OnClick()
    {
        if(_researchSamplesSystem.Service.AddItem(_sampleSO.Id, 1))
        {
            Pickup();
        }
    }

    private void Pickup(){
        AudioManagerGlobal.Instance.PlayOneShot("rewardLight");
        _itemPickupAnimatorRef.Service.AnimatePickup(this);
            Destroy(gameObject);
    }

    private void OnValidate()
    {
        if(_sampleSO != null)
        {
            _renderer.sprite = _sampleSO.Sprite;
        }
    }

}
