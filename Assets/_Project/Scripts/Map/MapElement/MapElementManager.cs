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

    protected TElement _nowElement;
    protected MapData _mapData;
    protected readonly List<TElement> _mapElements = new();

    public TElement NowElement => _nowElement;

    public event Action<bool> StateChanged;

    public virtual void GenerateMapElements(MapData mapData) {
        _mapData = mapData;
        foreach (var elementData in GetElementsData())
            GenerateMapElement(elementData);
    }

    public virtual void GenerateMapElement(TElementData elementData) {
        TElement mapElement = _pool.GetObject();
        mapElement.Setup(_mapData, elementData);
        _mapElements.Add(mapElement);
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

    protected abstract TEditableData GetEditableById(int id);
    protected abstract IEnumerable<TElementData> GetElementsData();
    protected abstract IEnumerable<TEditableData> GetEditablesData(MapData mapData);
}
