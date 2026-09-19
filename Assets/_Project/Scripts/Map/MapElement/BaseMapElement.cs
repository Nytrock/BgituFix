using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMapElement : MonoBehaviour, IResolvable {
    protected List<BaseEditable> _editables = new();

    protected float _cameraSize;
    protected float _precision;
    protected MapData _mapData;

    public float CameraSize => _cameraSize;
    public float Precision => _precision;
    public int Id => BaseData.Id;

    public IdData BaseData { get; private set; }

    public virtual void Setup(MapData mapData, IdData data, float precision) {
        BaseData = data;
        _mapData = mapData;
        _precision = precision;
        GenerateEditables();
    }

    public void UpdateShowingSize(bool isShow) {
        if (!gameObject.activeSelf)
            return;

        ChangeSizeShowState(isShow);
    }

    public virtual void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }

    protected virtual void GenerateEditables() {
        foreach (var data in GetEditablesData(_mapData))
            GenerateEditable(data);
    }

    public virtual BaseEditable GenerateEditable(EditableData data, bool neeedOverlapCheck = false) {
        BaseEditable editable = GetNewEditable();
        editable.Setup(data, this);
        _editables.Add(editable);
        if (neeedOverlapCheck)
            CheckEditableOverlap(editable);
        return editable;
    }

    private void CheckEditableOverlap(BaseEditable checkingEditable) {
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

    public BaseEditable CreateEmptyEditable() {
        EditableData data = GenerateEmptyEditableData();
        BaseEditable editable = GenerateEditable(data, true);

        editable.ChangeSelectState();
        return editable;
    }

    public void DeleteEditable(EditableData editableData) {
        BaseEditable editable = FindEditableById(editableData.Id);
        if (editable != null)
            DeleteEditable(editable);
    }

    public virtual void DeleteEditable(BaseEditable editable) {
        if (!_editables.Contains(editable))
            return;

        _editables.Remove(editable);
    }

    public void ChangeEditable(EditableData editableData) {
        BaseEditable editable = FindEditableById(editableData.Id);
        if (editable != null)
            editable.SetData(editableData);
    }

    protected BaseEditable FindEditableById(int id) {
        foreach (var editable in _editables)
            if (editable.BaseData.Id == id)
                return editable;
        return null;
    }

    protected abstract IEnumerable<EditableData> GetEditablesData(MapData mapData);
    protected abstract BaseEditable GetNewEditable();
    protected abstract void ChangeSizeShowState(bool newState);
    public abstract void GetBounds(out Vector2 leftDown, out Vector2 rightUp);
    protected abstract EditableData GenerateEmptyEditableData();
    public abstract void Inject(ObjectResolver resolver);
}
