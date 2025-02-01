using UnityEngine;

public class MapEditorManagerUI : MonoBehaviour {
    [SerializeField] private UserManager _userManager;
    [SerializeField] private MapEditorManager _editorManager;
    [SerializeField] private GameObject _globalPanel;
    [SerializeField] private GameObject _defaultPanel;
    [SerializeField] private GameObject _editPanel;

    private void Start() {
        ChangePermission(_userManager.GetUserType() == UserType.Admin);
        ChangeEditState(false);
    }

    private void ChangePermission(bool newState) {
        _globalPanel.SetActive(newState);
    }

    private void ChangeEditState(bool newState) {
        _defaultPanel.SetActive(!newState);
        _editPanel.SetActive(newState);
    }
}
