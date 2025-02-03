using System;
using UnityEngine;

public class MapEditManager : MonoBehaviour {
    [SerializeField] private UserManager _userManager;
    [SerializeField] private MapManager _mapManager;

    private bool _isAdmin;
    private bool _isEdit;
    private MapData _oldMapData;
    private BaseEditable _nowEditable;

    public event Action<bool> EditStateChanged;
    public event Action<bool> PermissionChanged;

    public bool IsEdit => _isEdit;

    private void Start() {
        ChangePermission(_userManager.ClientType == UserType.Admin);
        ChangeState(false);
    }

    private void ChangePermission(bool isAdmin) {
        _isAdmin = isAdmin;
        PermissionChanged?.Invoke(isAdmin);
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
            return;
        }

        if (_nowEditable != null)
            _nowEditable.ChangeEditingMode(false);
        _nowEditable = editable;
        _nowEditable.ChangeEditingMode(true);
    }
}
