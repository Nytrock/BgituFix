using System;
using UnityEngine;

public class MapEditorManager : MonoBehaviour {
    [SerializeField] private UserManager _userManager;

    private bool _isAdmin;

    public event Action<bool> EditStateChanged;
    public event Action<bool> PermissionChanged;

    private void Start() {
        ChangePermission(_userManager.ClientType == UserType.Admin);
        ChangeState(false);
    }

    private void ChangePermission(bool isAdmin) {
        _isAdmin = isAdmin;
        PermissionChanged?.Invoke(isAdmin);
    }

    public void StartEdit() {
        ChangeState(true);
    }

    public void SaveChanges() {
        ChangeState(false);
    }

    public void CancelChanges() {
        ChangeState(false);
    }

    private void ChangeState(bool newState) {
        if (!_isAdmin)
            return;

        EditStateChanged?.Invoke(newState);
    }
}
