using UnityEngine;

public class FlickeringStar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    private const float MIN_SCALE = 0.33f;
    private const float MAX_SCALE = 0.66f;

    private ComponentPool<FlickeringStar> _pool;
    protected virtual void OnEnable()
    {
        float randScale = Random.Range(MIN_SCALE, MAX_SCALE);
        _renderer.transform.localScale = Vector3.one * randScale;
    }

    public void Initialize(ComponentPool<FlickeringStar> pool)
    {
        _pool = pool;
    }

    public void OnAnimationEnd()
    {
        _pool.Release(this);
    }
}
