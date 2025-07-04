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

    public virtual void Setup(MapData mapData, TData data, float precision) {
        _data = data;
        _mapData = mapData;
        _precision = precision;
        GenerateEditables();
    }

    protected virtual void GenerateEditables() {
        foreach (var data in GetEditablesData(_mapData))
            GenerateEditable(data);
    }

    public virtual TEditable GenerateEditable(TEditableData data, bool neeedOverlapCheck = false) {
        TEditable editable = _pool.GetObject();
        editable.Setup(data, this);
        _editables.Add(editable);
        if (neeedOverlapCheck)
            CheckEditableOverlap(editable);
        return editable;
    }

    private void CheckEditableOverlap(TEditable checkingEditable) {
        Vector2 offset = new(_precision / 2f, -_precision / 2f);

        while (true) {
            bool overlap = false;
            foreach (var editable in _editables) {
                if (editable == checkingEditable) continue;

                if (editable.Position == checkingEditable.Position) {

                    overlap = true;
                    checkingEditable.ChangePosition(checkingEditable.Position + offset);
                }
            }

            if (!overlap) break;
        }
    }

    public TEditable CreateEmptyEditable() {
        TEditableData data = GenerateEmptyEditableData();
        TEditable editable = GenerateEditable(data, true);

        editable.ChangeSelectState();
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
