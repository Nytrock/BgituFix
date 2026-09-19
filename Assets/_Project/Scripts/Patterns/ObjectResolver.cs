using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectResolver : MonoBehaviour {
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private MapEditManager _editManager;
    [SerializeField] private UserManager _userManager;
    [SerializeField] private ErrorManager _errorManager;
    [SerializeField] private SelectManager _selectManager;
    [SerializeField] private ComputerUIManager _computerUIManager;

    private Dictionary<Type, object> _instances;

    private void Start() {
        AddInstance(_mapManager);
        AddInstance(_editManager);
        AddInstance(_errorManager);
        AddInstance(_userManager);
        AddInstance(_selectManager);
        AddInstance(_computerUIManager);
    }

    private void AddInstance<T>(T instance) {
        _instances.Add(typeof(T), instance);
    }

    public T GetInstance<T>() {
        Type type = typeof(T);
        if (_instances.TryGetValue(type, out object value))
            return (T)value;

        return default;
    }
}
