using System;
using System.Collections.Generic;
using UnityEngine;

public class MapEditManager : MonoBehaviour {
    [SerializeField] private GameObject _submitChangesPanel;
    [SerializeField] private UserManager _userManager;
    [SerializeField] private RulerManager _rulerManager;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private HelpManager _helpManager;
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
    public bool IsRulerActive => _rulerManager.IsActive;

    private void Awake() {
        _mapManager.MapGenerated += delegate { EndEdit(); };
        _userManager.ClientSetuped += delegate { SetPermission(); };
        _rulerManager.StateChanged += CheckRulerState;
    }

    private void CheckRulerState() {
        if (_rulerManager.IsActive)
            DeselectAllNowEditables();
    }

    private void SetPermission() {
        _isAdmin = _userManager.ClientType == UserType.Admin;
        PermissionChanged?.Invoke(_isAdmin);
    }

    public void StartEdit() {
        _isEdit = true;
        ChangeState(true);
        _gridManager.SetPrecision(_mapManager.GetPrecision());
        SetOldMapData();
    }

    private void SetOldMapData() {
        string oldMapDataJson = JsonUtility.ToJson(_mapManager.Data);
        _oldMapData = JsonUtility.FromJson<MapData>(oldMapDataJson);
        _oldMapData.SetupVectors();
    }

    public void EndEdit() {
        _isEdit = false;
        CloseSubmitPanel();
        ChangeState(false);

        _oldMapData = null;
        if (_nowEditables.Count != 0)
            DeselectAllNowEditables();
        _nowEditables.Clear();
        _clipboard.Clear();
        _mapManager.UpdateLocationSizeShow(false);
        _rulerManager.ChangeState(false);
    }

    public void OpenSubmitPanel() {
        if (_oldMapData.Equals(_mapManager.Data)) {
            EndEdit();
            return;
        }

        ChangeSubmitPanelState(true);
    }

    public void CloseSubmitPanel() {
        ChangeSubmitPanelState(false);
    }

    private void ChangeSubmitPanelState(bool newState) {
        _submitChangesPanel.SetActive(newState);
    }

    public void SaveChangesWithoutEndingEdit() {
        StartCoroutine(_mapManager.SubmitMapDataChanges(_oldMapData));
        SetOldMapData();
    }

    public void SaveChanges() {
        StartCoroutine(_mapManager.SubmitMapDataChanges(_oldMapData));
        EndEdit();
    }

    public void CancelChanges() {
        _mapManager.RevertMapDataChanges(_oldMapData);
        EndEdit();
    }

    private void ChangeState(bool newState) {
        _helpManager.ChangeState(false);
        _cameraManager.ChangeEditState(newState);
        _gridManager.ChangeState(newState);
        EditStateChanged?.Invoke(newState);
    }

    public void ChangeSelectStateOfSingleEditable(BaseEditable editable) {
        ChangeCameraMoving(true);

        if (_nowEditables.Count == 1 && _nowEditables[0] == editable) {
            DeselectAllNowEditables();
        } else {
            if (_nowEditables.Count != 0)
                DeselectAllNowEditables();
            _nowEditables.Add(editable);
            editable.ChangeEditingState(true);
        }

        _mapManager.UpdateLocationSizeShow(_nowEditables.Count != 0);
    }

    public void ChangeSelectStateOfAdditionalEditable(BaseEditable editable) {
        ChangeCameraMoving(true);

        if (_nowEditables.Contains(editable)) {
            _nowEditables.Remove(editable);
            editable.ChangeEditingState(false);
        } else {
            _nowEditables.Add(editable);
            editable.ChangeEditingState(true);
        }

        _mapManager.UpdateLocationSizeShow(_nowEditables.Count != 0);
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
        DeselectAllNowEditables();
    }

    public void ChangeCameraMoving(bool isMoving) {
        _cameraManager.ChangeMoveState(isMoving);
    }

    public void DeselectAllNowEditables() {
        _nowEditables.ForEach(editable => {
            editable.ChangeEditingState(false);
            EditableRemoved?.Invoke(editable);
        });
        _nowEditables.Clear();
        _mapManager.UpdateLocationSizeShow(false);
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
        _mapManager.UpdateLocationSizeShow(true);
    }

    public bool IsEditableSelected(BaseEditable editable) {
        return _nowEditables.Contains(editable);
    }
}
