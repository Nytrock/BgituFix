using UnityEngine;

public class MapEditManagerUI : MonoBehaviour {
    [SerializeField] private MapEditManager _editManager;
    [SerializeField] private GameObject _permissionPanel;
    [SerializeField] private GameObject _nonPermissionPanel;
    [SerializeField] private GameObject _defaultPanel;
    [SerializeField] private GameObject _editPanel;

    private void Awake() {
        _editManager.PermissionChanged += ChangePermission;
        _editManager.EditStateChanged += ChangeEditState;
    }

    private void ChangePermission(bool newState) {
        _permissionPanel.SetActive(newState);
        if (_nonPermissionPanel != null)
            _nonPermissionPanel.SetActive(!newState);
    }

    private void ChangeEditState(bool newState) {
        _defaultPanel.SetActive(!newState);
        _editPanel.SetActive(newState);
    }
}
