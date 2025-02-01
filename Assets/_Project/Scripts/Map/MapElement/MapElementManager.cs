using System;
using System.Collections.Generic;
using UnityEngine;

public class MapElementManager<TElement, TEditable, TEditableData, TData> : MonoBehaviour
    where TElement : MapElement<TEditable, TEditableData, TData>
    where TEditable : Editable<TEditableData>
    where TEditableData : EditableData {

    [SerializeField] protected Pool<TElement> _mapElementsPool;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private float _cameraSizeOffset;

    protected TElement _nowElement;
    protected readonly List<TElement> _mapElements = new();

    public event Action<bool> StateChanged;

    protected void UpdateCameraSize() {
        float newSize = _nowElement.CameraSize;
        _cameraManager.ForceSetSize(newSize + newSize / _cameraSizeOffset);
    }

    public virtual void ChangeState(bool newState) {
        gameObject.SetActive(newState);
        StateChanged?.Invoke(newState);
    }

    protected virtual void UpdateElement(TElement newElememt) {
        if (_nowElement != null)
            _nowElement.ChangeState(false);
        _nowElement = newElememt;
        _nowElement.ChangeState(true);
        UpdateCameraSize();
    }
}
