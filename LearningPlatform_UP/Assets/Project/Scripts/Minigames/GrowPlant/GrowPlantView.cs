using UnityEngine;

public class GrowPlantView : MonoBehaviour
{
    [SerializeField] private Transform _plantTransform;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _growTriggerName = "Grow";

    public Transform Target => _plantTransform == null ? transform : _plantTransform;

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }

    public void Grow()
    {
        if (_animator == null || string.IsNullOrWhiteSpace(_growTriggerName))
        {
            return;
        }

        _animator.SetTrigger(_growTriggerName);
    }
}
