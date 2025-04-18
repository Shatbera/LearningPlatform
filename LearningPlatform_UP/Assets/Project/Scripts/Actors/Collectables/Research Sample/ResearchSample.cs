using UnityEngine;

public class ResearchSample : ClickableObject, ICollectableItem
{
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

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private ItemCollectEventChannel _collectEventChannel;
    private void Start()
    {
        _animator.SetFloat("Offset", Random.Range(0f, 1f));
        _animator.SetInteger("Animation", (int)_animation);
    }
    public override void OnClick()
    {
        _collectEventChannel.Raise(new ItemCollectEventData { Item = this, Amount = 1} );
    }

    public void OnPickup()
    {
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
