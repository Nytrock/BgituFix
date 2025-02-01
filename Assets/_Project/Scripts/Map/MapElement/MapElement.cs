using System.Collections.Generic;
using UnityEngine;

public abstract class MapElement<TEditable, TEditableData, TData> : MonoBehaviour
    where TEditable : Editable<TEditableData>
    where TEditableData : EditableData {

    [SerializeField] private Pool<TEditable> _pool;

    protected TData _data;
    protected float _cameraSize;

    public float CameraSize => _cameraSize;

    public virtual void Setup(MapData mapData, TData data) {
        _data = data;
        GenerateEditables(mapData);
    }

    protected virtual void GenerateEditables(MapData mapData) {
        foreach (var data in GetEditablesData(mapData))
            GenerateEditable(data);
    }

    protected virtual TEditable GenerateEditable(TEditableData data) {
        TEditable editable = _pool.GetObject();
        editable.Setup(data);
        return editable;
    }

    public virtual void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }

    protected abstract IEnumerable<TEditableData> GetEditablesData(MapData mapData);
}
