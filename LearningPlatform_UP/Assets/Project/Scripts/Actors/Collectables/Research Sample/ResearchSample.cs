using System.Collections;
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

    private const float MAX_ANIMATION_START_DELAY = 2f;
    public CollectableItemSO ItemSO => _sampleSO;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _animator;
    private void Start()
    {
        StartCoroutine(SetAnimationCoroutine());
    }
    private IEnumerator SetAnimationCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(0, MAX_ANIMATION_START_DELAY));
        _animator.SetInteger("Animation", (int)_animation);
    }
    public override void OnClick()
    {
        ICollectableItem.RequestPickup(this, 1);
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
