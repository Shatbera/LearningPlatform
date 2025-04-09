using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class ComponentPool<T> : IObjectPool<T> where T : Component
{
    [SerializeField] private Transform parent;
    [SerializeField] private T prefab;
    [SerializeField] private int defaultCapacity = 10;

    public Transform Parent => parent;

    private ObjectPool<T> pool;
    public int CountInactive => pool.CountInactive;

    public ObjectPool<T> Pool => pool;
    public ComponentPool()
    {
        pool = new ObjectPool<T>(CreateObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, defaultCapacity);
    }

    private T CreateObject()
    {
        var instance = Object.Instantiate(prefab, parent);
        instance.gameObject.SetActive(false);
        return instance;
    }

    private void OnTakeFromPool(T instance)
    {
        instance.gameObject.SetActive(true);
    }

    private void OnReturnToPool(T instance)
    {
        instance.gameObject.SetActive(false);
    }

    private void OnDestroyObject(T instance)
    {
        Object.Destroy(instance.gameObject);
    }

    public T Get()
    {
        return pool.Get();
    }

    public PooledObject<T> Get(out T v)
    {
        return pool.Get(out v);
    }

    public void Release(T element)
    {
        pool.Release(element);
    }

    public void Clear()
    {
        pool.Clear();
    }
}
