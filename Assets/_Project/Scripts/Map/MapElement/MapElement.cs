using System.Collections.Generic;
using UnityEngine;

public abstract class MapElement<TEditable, TEditableData, TData> : BaseMapElement
    where TEditable : Editable<TEditableData>
    where TEditableData : EditableData
    where TData : IdData {

    [SerializeField] protected Pool<TEditable> _pool;

    protected TData _data;
    protected MapData _mapData;
    protected List<TEditable> _editables = new();

    public TData Data => _data;
    public int Id => _data.Id;

    public virtual void Setup(MapData mapData, TData data) {
        _data = data;
        _mapData = mapData;
        GenerateEditables();
    }

    protected virtual void GenerateEditables() {
        foreach (var data in GetEditablesData(_mapData))
            GenerateEditable(data);
    }

    public virtual TEditable GenerateEditable(TEditableData data) {
        TEditable editable = _pool.GetObject();
        editable.Setup(data);
        _editables.Add(editable);
        return editable;
    }

    public void DeleteEditable(TEditableData editableData) {
        TEditable editable = FindEditableById(editableData.Id);
        if (editable != null)
            DeleteEditable(editable);
    }

    public virtual void DeleteEditable(TEditable editable) {
        if (!_editables.Contains(editable))
            return;

        _editables.Remove(editable);
        _pool.PutObject(editable);
    }

    public void ChangeEditable(TEditableData editableData) {
        TEditable editable = FindEditableById(editableData.Id);
        if (editable != null)
            editable.SetData(editableData);
    }

    protected TEditable FindEditableById(int id) {
        foreach (var editable in _editables)
            if (editable.Data.Id == id)
                return editable;
        return null;
    }

    protected abstract IEnumerable<TEditableData> GetEditablesData(MapData mapData);
    protected abstract TEditableData GenerateEmptyEditableData();
}
