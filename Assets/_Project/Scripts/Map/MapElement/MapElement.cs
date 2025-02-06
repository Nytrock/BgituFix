using System.Collections.Generic;
using UnityEngine;

public abstract class MapElement<TEditable, TEditableData, TData> : MonoBehaviour
    where TEditable : Editable<TEditableData>
    where TEditableData : EditableData {

    [SerializeField] protected Pool<TEditable> _pool;

    protected TData _data;
    protected float _cameraSize;
    protected List<TEditable> _editables = new();

    public float CameraSize => _cameraSize;
    public TData Data => _data;
    public abstract int Id { get; }

    public virtual void Setup(MapData mapData, TData data) {
        _data = data;
        GenerateEditables(mapData);
    }

    protected virtual void GenerateEditables(MapData mapData) {
        foreach (var data in GetEditablesData(mapData))
            GenerateEditable(data);
    }

    protected void UpdateShowingSize(BaseEditable editable) {
        if (!gameObject.activeSelf)
            return;

        bool isShowSizes = _editables.Contains(editable as TEditable);
        ChangeSizeShowState(isShowSizes);
    }

    protected void UpdateShowingSize(bool isEdit) {
        if (!gameObject.activeSelf)
            return;

        if (!isEdit)
            ChangeSizeShowState(false);
    }

    public virtual TEditable GenerateEditable(TEditableData data) {
        TEditable editable = _pool.GetObject();
        editable.Setup(data);
        _editables.Add(editable);
        return editable;
    }

    public virtual void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }

    public virtual TEditable CreateEditable(TEditableData data) {
        return GenerateEditable(data);
    }

    public void DeleteEditable(TEditable editable) {
        if (!_editables.Contains(editable))
            return;

        _editables.Remove(editable);
        _pool.PutObject(editable);
    }

    protected abstract void ChangeSizeShowState(bool newState);
    protected abstract IEnumerable<TEditableData> GetEditablesData(MapData mapData);
}
