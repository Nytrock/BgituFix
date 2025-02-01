using System.Collections.Generic;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : MonoBehaviour {
    [SerializeField] private T _prefab;
    private readonly Queue<T> _pool = new();

    protected virtual T CreateObject() {
        return Instantiate(_prefab, transform);
    }

    public virtual T GetObject() {
        if (_pool.Count == 0)
            _pool.Enqueue(CreateObject());

        T obj = _pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public virtual void PutObject(T obj) {
        if (_pool.Contains(obj))
            return;

        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
