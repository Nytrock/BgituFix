using System.Collections.Generic;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : MonoBehaviour {
    [SerializeField] private T _prefab;
    private readonly Queue<T> _pool = new();

    public virtual T GetObject() {
        if (_pool.Count == 0)
            _pool.Enqueue(Instantiate(_prefab, transform));

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
