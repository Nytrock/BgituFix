using System;
using System.Collections.Generic;
using UnityEngine;

public class MapEditManager : MonoBehaviour {
    [SerializeField] private UserManager _userManager;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private CameraManager _cameraManager;

    private bool _isAdmin;
    private bool _isEdit;
    private MapData _oldMapData;

    private readonly List<BaseEditable> _nowEditables = new();
    private readonly List<EditableData> _clipboard = new();

    public event Action<bool> EditStateChanged;
    public event Action<bool> PermissionChanged;
    public event Action<BaseEditable> EditableRemoved;

    public bool IsEdit => _isEdit;

    private void Awake() {
        _mapManager.MapGenerated += delegate { ChangeState(false); };
        _userManager.ClientSetuped += delegate { SetPermission(); };
    }

    private void SetPermission() {
        _isAdmin = _userManager.ClientType == UserType.Admin;
        PermissionChanged?.Invoke(_isAdmin);
    }

    public void StartEdit() {
        _isEdit = true;
        ChangeState(true);

        string oldMapDataJson = JsonUtility.ToJson(_mapManager.Data);
        _oldMapData = JsonUtility.FromJson<MapData>(oldMapDataJson);
        _oldMapData.SetupVectors();
    }

    public void SaveChanges() {
        _isEdit = false;
        StartCoroutine(_mapManager.SubmitMapDataChanges(_oldMapData));
        ChangeState(false);
    }

    public void CancelChanges() {
        _isEdit = false;
        _mapManager.RevertMapDataChanges(_oldMapData);
        ChangeState(false);
    }

    private void ChangeState(bool newState) {
        if (!newState) {
            _oldMapData = null;
            if (_nowEditables.Count != 0)
                DeselectAllNowEditables();
            _nowEditables.Clear();
            _clipboard.Clear();
        }
        EditStateChanged?.Invoke(newState);
    }

    public void ChangeSelectStateOfSingleEditable(BaseEditable editable) {
        ChangeCameraMoving(true);

        if (_nowEditables.Count == 1 && _nowEditables[0] == editable) {
            DeselectAllNowEditables();
            return;
        }

        if (_nowEditables.Count != 0)
            DeselectAllNowEditables();
        _nowEditables.Add(editable);
        editable.ChangeEditingState(true);
    }

    public void ChangeSelectStateOfAdditionalEditable(BaseEditable editable) {
        ChangeCameraMoving(true);

        if (_nowEditables.Contains(editable)) {
            _nowEditables.Remove(editable);
            editable.ChangeEditingState(false);
            return;
        }

        _nowEditables.Add(editable);
        editable.ChangeEditingState(true);
    }

    public void ChangeEditablesPosition(Vector3 offset) {
        foreach (var editable in _nowEditables)
            editable.ChangePosition(editable.transform.position + offset);
    }

    public void CreateEmptyEditable() {
        _mapManager.CreateEmptyEditable();
    }

    public void Copy() {
        _clipboard.Clear();
        foreach (var editable in _nowEditables) {
            if (_mapManager.State == MapState.Build) {
                EditableAudience audience = editable as EditableAudience;
                _clipboard.Add(new AudienceData(audience.Data));
            } else {
                EditableComputer computer = editable as EditableComputer;
                _clipboard.Add(new ComputerData(computer.Data));
            }
        }
    }

    public void Paste() {
        if (_clipboard.Count == 0)
            return;

        IEnumerable<BaseEditable> pastedEditables = _mapManager.PasteEditables(_clipboard);
        DeselectAllNowEditables();

        foreach (var editable in pastedEditables)
            ChangeSelectStateOfAdditionalEditable(editable);
    }

    public void Delete() {
        _mapManager.DeleteEditables(_nowEditables);
        _nowEditables.Clear();
    }

    public void ChangeCameraMoving(bool isMoving) {
        _cameraManager.ChangeMovingState(isMoving);
    }

    public void DeselectAllNowEditables() {
        _nowEditables.ForEach(editable => {
            editable.ChangeEditingState(false);
            EditableRemoved?.Invoke(editable);
        });
        _nowEditables.Clear();
    }

    public void DeselectAllNowEditablesExceptOne(BaseEditable singleEditable) {
        if (_nowEditables.Count <= 1)
            return;

        _nowEditables.ForEach(editable => {
            if (editable != singleEditable) {
                editable.ChangeEditingState(false);
                EditableRemoved?.Invoke(editable);
            }
        });
        _nowEditables.Clear();
        _nowEditables.Add(singleEditable);
    }

    public bool IsEditableSelected(BaseEditable editable) {
        return _nowEditables.Contains(editable);
    }
}
