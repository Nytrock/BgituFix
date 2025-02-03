using UnityEngine;

public class MapEditorManagerUI : MonoBehaviour {
    [SerializeField] private MapEditorManager _editorManager;
    [SerializeField] private GameObject _globalPanel;
    [SerializeField] private GameObject _defaultPanel;
    [SerializeField] private GameObject _editPanel;

    private void Awake() {
        _editorManager.PermissionChanged += ChangePermission;
        _editorManager.EditStateChanged += ChangeEditState;
    }

    private void ChangePermission(bool newState) {
        _globalPanel.SetActive(newState);
    }

    private void ChangeEditState(bool newState) {
        _defaultPanel.SetActive(!newState);
        _editPanel.SetActive(newState);
    }
}
