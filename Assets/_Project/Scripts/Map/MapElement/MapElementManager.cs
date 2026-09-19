using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class MapElementManager<TElement> : MonoBehaviour where TElement : BaseMapElement {
    [SerializeField] protected ResolverPool<TElement> _pool;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private float _gridPrecision;
    [SerializeField] private string _APIPathForEditables;

    protected TElement _nowElement;
    protected MapData _mapData;
    protected readonly List<TElement> _mapElements = new();

    public TElement NowElement => _nowElement;
    public float GridPrecision => _gridPrecision;

    public event Action<bool> StateChanged;

    public virtual void GenerateMapElements(MapData mapData) {
        _mapData = mapData;
        foreach (var elementData in GetElementsData())
            GenerateMapElement(elementData);
    }

    public virtual void GenerateMapElement(IdData elementData) {
        TElement mapElement = _pool.GetObject();
        mapElement.Setup(_mapData, elementData, _gridPrecision);
        _mapElements.Add(mapElement);
    }

    public void UpdateNowElementShowingSize(bool isShow) {
        _nowElement.UpdateShowingSize(isShow);
    }

    protected void UpdateCamera() {
        float newSize = _nowElement.CameraSize;
        _cameraManager.ForceSetSize(newSize);
        _cameraManager.ResetPosition();
    }

    public virtual void ChangeState(bool newState) {
        gameObject.SetActive(newState);
        StateChanged?.Invoke(newState);
    }

    public IdData GetElementDataById(int id) {
        foreach (var element in _mapElements)
            if (element.Id == id)
                return element.BaseData;
        return null;
    }

    protected virtual void SelectElement(TElement newElememt) {
        _nowElement?.ChangeState(false);
        _nowElement = newElememt;
        _nowElement.ChangeState(true);
        UpdateCamera();
    }

    public void DeleteEditableOnMap(BaseEditable editable) {
        _nowElement.DeleteEditable(editable);
        _mapData.DeleteEditable(editable.BaseData);
    }

    public void DeleteEditableOnMap(EditableData editableData) {
        _nowElement.DeleteEditable(editableData);
    }

    public BaseEditable CreateEditableOnMap(EditableData editableData, bool needOverlapCheck = false) {
        return _nowElement.GenerateEditable(editableData, needOverlapCheck);
    }

    public BaseEditable CreateEmptyEditableOnMap() {
        return _nowElement.CreateEmptyEditable();
    }

    public virtual void ChangeEditableOnMap(EditableData editableData) {
        _nowElement.ChangeEditable(editableData);
    }

    public void RevertEditingChanges(MapData oldMapData) {
        foreach (var oldData in GetEditablesData(oldMapData)) {
            if (!_mapData.ContainsEditable(oldData))
                CreateEditableOnMap(oldData);
            else
                ChangeEditableOnMap(oldData);
        }

        foreach (var newData in GetEditablesData(_mapData))
            if (!oldMapData.ContainsEditable(newData))
                DeleteEditableOnMap(newData);
    }

    public IEnumerator SubmitEditingChanges(MapData oldMapData) {
        foreach (var oldData in GetEditablesData(oldMapData)) {
            if (!_mapData.ContainsEditable(oldData)) {
                yield return DeleteEditableAfterEditing(oldData);
            } else {
                EditableData newData = GetEditableById(oldData.Id);
                if (!newData.Equals(oldData))
                    yield return ChangeEditableAfterEditing(newData);
            }
        }

        foreach (var newData in GetEditablesData(_mapData))
            if (newData.Id == -1)
                yield return CreateEditableAfterEditing(newData);
    }

    public virtual IEnumerator DeleteEditableAfterEditing(EditableData editableData) {
        UnityWebRequest request = APIUtility.Delete(_APIPathForEditables, editableData.Id);
        yield return request.SendWebRequestSafely();
    }

    public virtual IEnumerator CreateEditableAfterEditing(EditableData editableData) {
        UnityWebRequest request = APIUtility.Post(_APIPathForEditables, editableData);
        yield return request.SendWebRequestSafely();

        IdData idData = request.ToData<IdData>();
        editableData.SetId(idData.Id);
    }

    public virtual IEnumerator ChangeEditableAfterEditing(EditableData editableData) {
        UnityWebRequest request = APIUtility.Put(_APIPathForEditables, editableData, editableData.Id);
        yield return request.SendWebRequestSafely();
    }

    protected abstract EditableData GetEditableById(int id);
    protected abstract IEnumerable<IdData> GetElementsData();
    protected abstract IEnumerable<EditableData> GetEditablesData(MapData mapData);
}
