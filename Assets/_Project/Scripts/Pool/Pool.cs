using System.Collections.Generic;
using UnityEngine;

public class Pool<T> : MonoBehaviour where T : MonoBehaviour {
    [SerializeField] private T _prefab;

    private Transform _container;
    private Queue<T> _pool;

    private void Awake() {
        _pool = new();
        _container = transform;
    }

    public T GetObject() {
        if (_pool.Count == 0)
            _pool.Enqueue(Instantiate(_prefab, _container));

        T obj = _pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void PutObject(T obj) {
        if (_pool.Contains(obj))
            return;

        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
