using System;
using UnityEngine;

public class MapEditManager : MonoBehaviour {
    [SerializeField] private UserManager _userManager;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private CameraManager _cameraManager;

    private bool _isAdmin;
    private bool _isEdit;
    private MapData _oldMapData;
    private BaseEditable _nowEditable;

    public event Action<BaseEditable> EditableChanged;
    public event Action<bool> EditStateChanged;
    public event Action<bool> PermissionChanged;

    public bool IsEdit => _isEdit;

    private void Awake() {
        _userManager.ClientSetuped += delegate { ChangePermission(); };
        _mapManager.MapGenerated += delegate { ChangeState(false); };
    }

    private void ChangePermission() {
        _isAdmin = _userManager.ClientType == UserType.Admin;
        PermissionChanged?.Invoke(_isAdmin);
    }

    public void StartEdit() {
        _isEdit = true;
        ChangeState(true);

        string oldMapDataJson = JsonUtility.ToJson(_mapManager.Data);
        _oldMapData = JsonUtility.FromJson<MapData>(oldMapDataJson);
    }

    public void SaveChanges() {
        _isEdit = false;
        StartCoroutine(_mapManager.CheckUpdatedData(_oldMapData));
        ChangeState(false);
    }

    public void CancelChanges() {
        _isEdit = false;
        StartCoroutine(_mapManager.RevertMapData(_oldMapData));
        ChangeState(false);
    }

    private void ChangeState(bool newState) {
        if (!_isAdmin)
            return;

        if (!newState) {
            _oldMapData = null;
            if (_nowEditable != null)
                _nowEditable.ChangeEditingMode(false);
            _nowEditable = null;
        }
        EditStateChanged?.Invoke(newState);
    }

    public void SetEditable(BaseEditable editable) {
        if (_nowEditable == editable) {
            _nowEditable.ChangeEditingMode(false);
            _nowEditable = null;
            EditableChanged?.Invoke(null);
            return;
        }

        if (_nowEditable != null)
            _nowEditable.ChangeEditingMode(false);
        _nowEditable = editable;
        _nowEditable.ChangeEditingMode(true);
        EditableChanged?.Invoke(_nowEditable);
    }

    public void CreateNewAudience() {
        EditableAudience audience = _mapManager.CreateNewAudience();
        SetEditable(audience);
    }

    public void CreateAudience(AudienceData audienceData) {
        EditableAudience audience = _mapManager.CreateAudience(audienceData);
        SetEditable(audience);
    }

    public void DeleteAudience(EditableAudience audience) {
        if (_nowEditable == audience)
            SetEditable(audience);
        _mapManager.DeleteAudience(audience);
    }

    public void CreateNewComputer() {
        EditableComputer computer = _mapManager.CreateNewComputer();
        SetEditable(computer);
    }

    public void CreateComputer(ComputerData computerData) {
        EditableComputer computer = _mapManager.CreateComputer(computerData);
        SetEditable(computer);
    }

    public void DeleteComputer(EditableComputer computer) {
        if (_nowEditable == computer)
            SetEditable(computer);
        _mapManager.DeleteComputer(computer);
    }

    public void ChangeCameraMoving(bool isMoving) {
        _cameraManager.ChangeMovingState(isMoving);
    }
}
