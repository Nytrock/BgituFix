using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class MapElementManager<TElement, TEditable, TEditableData, TData> : MonoBehaviour
    where TElement : MapElement<TEditable, TEditableData, TData>
    where TEditable : Editable<TEditableData>
    where TEditableData : EditableData
    where TData : class {

    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private LoginManager _loginManager;
    [SerializeField] private string _APIPathToCreateEditable;
    [SerializeField] private string _APIPathToChangeEditable;
    [SerializeField] private string _APIPathToDeleteEditable;

    protected TElement _nowElement;
    protected readonly List<TElement> _mapElements = new();

    public TElement NowElement => _nowElement;

    public event Action<bool> StateChanged;

    protected void UpdateCamera() {
        float newSize = _nowElement.CameraSize;
        _cameraManager.ForceSetSize(newSize);
        _cameraManager.ResetPosition();
    }

    public virtual void ChangeState(bool newState) {
        gameObject.SetActive(newState);
        StateChanged?.Invoke(newState);
    }

    public TData GetElementDataById(int id) {
        foreach (var element in _mapElements)
            if (element.Id == id)
                return element.Data;
        return null;
    }

    protected virtual void UpdateElement(TElement newElememt) {
        if (_nowElement != null)
            _nowElement.ChangeState(false);
        _nowElement = newElememt;
        _nowElement.ChangeState(true);
        UpdateCamera();
    }

    public void DeleteEditable(TEditable editable) {
        _nowElement.DeleteEditable(editable);
    }

    public TEditable CreateEditable(TEditableData editableData) {
        return _nowElement.CreateEditable(editableData);
    }

    public IEnumerator DeleteEditableInDatabase(TEditableData editableData) {
        string token = _loginManager.Token;
        UnityWebRequest request = RequestUtility.APIDelete(_APIPathToDeleteEditable, editableData.Id, token);
        yield return request.SendWebRequest();
    }

    public IEnumerator CreateEditableInDatabase(TEditableData editableData) {
        string token = _loginManager.Token;
        UnityWebRequest request = RequestUtility.APIPost(_APIPathToCreateEditable, editableData, token);
        yield return request.SendWebRequest();

        IdData idData = request.ToData<IdData>();
        editableData.SetId(idData.Id);
    }

    public IEnumerator ChangeEditableInDatabase(TEditableData editableData) {
        string token = _loginManager.Token;
        UnityWebRequest request = RequestUtility.APIPut(_APIPathToChangeEditable, editableData, token);
        yield return request.SendWebRequest();
    }
}
