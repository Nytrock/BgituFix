using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapElementManager<TElement, TEditable, TEditableData, TElementData> : MonoBehaviour
    where TElement : MapElement<TEditable, TEditableData, TElementData>
    where TEditable : Editable<TEditableData>
    where TEditableData : EditableData
    where TElementData : IdData {

    [SerializeField] protected Pool<TElement> _pool;
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

    public virtual void GenerateMapElement(TElementData elementData) {
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

    public TElementData GetElementDataById(int id) {
        foreach (var element in _mapElements)
            if (element.Id == id)
                return element.Data;
        return null;
    }

    protected virtual void SelectElement(TElement newElememt) {
        if (_nowElement != null)
            _nowElement.ChangeState(false);
        _nowElement = newElememt;
        _nowElement.ChangeState(true);
        UpdateCamera();
    }

    public void DeleteEditableOnMap(TEditable editable) {
        _nowElement.DeleteEditable(editable);
        _mapData.DeleteEditable(editable.Data);
    }

    public void DeleteEditableOnMap(TEditableData editableData) {
        _nowElement.DeleteEditable(editableData);
    }

    public TEditable CreateEditableOnMap(TEditableData editableData, bool needOverlapCheck = false) {
        return _nowElement.GenerateEditable(editableData, needOverlapCheck);
    }

    public TEditable CreateEmptyEditableOnMap() {
        return _nowElement.CreateEmptyEditable();
    }

    public virtual void ChangeEditableOnMap(TEditableData editableData) {
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
                TEditableData newData = GetEditableById(oldData.Id);
                if (!newData.Equals(oldData))
                    yield return ChangeEditableAfterEditing(newData);
            }
        }

        foreach (var newData in GetEditablesData(_mapData))
            if (newData.Id == -1)
                yield return CreateEditableAfterEditing(newData);
    }

    public virtual IEnumerator DeleteEditableAfterEditing(TEditableData editableData) {
        UnityWebRequest request = APIUtility.Delete(_APIPathForEditables, editableData.Id);
        yield return request.SendWebRequestSafely();
    }

    public virtual IEnumerator CreateEditableAfterEditing(TEditableData editableData) {
        UnityWebRequest request = APIUtility.Post(_APIPathForEditables, editableData);
        yield return request.SendWebRequestSafely();

        IdData idData = request.ToData<IdData>();
        editableData.SetId(idData.Id);
    }

    public virtual IEnumerator ChangeEditableAfterEditing(TEditableData editableData) {
        UnityWebRequest request = APIUtility.Put(_APIPathForEditables, editableData, editableData.Id);
        yield return request.SendWebRequestSafely();
    }

    protected abstract TEditableData GetEditableById(int id);
    protected abstract IEnumerable<TElementData> GetElementsData();
    protected abstract IEnumerable<TEditableData> GetEditablesData(MapData mapData);
}
