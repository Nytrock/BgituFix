using UnityEngine;

public class ProfileManager : MonoBehaviour {
    [SerializeField] private GameObject _panel;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private LoginManager _loginManager;
    [SerializeField] private ErrorManager _errorManager;
    [SerializeField] private ProfileInfoRenderer _infoRenderer;
    [SerializeField] private ProfileErrorsRenderer _errorsRenderer;

    private void Awake() {
        ChangeState(false);
    }

    public void SetupProfile(UserData clientData) {
        _infoRenderer.SetInfo(clientData, _errorManager);
        _errorsRenderer.Setup(clientData, _errorManager);
    }

    public void Open() {
        ChangeState(true);
        _mapManager.ChangeState(false);
    }

    public void Close() {
        ChangeState(false);
        _mapManager.ChangeState(true);
    }

    private void ChangeState(bool newState) {
        _panel.SetActive(newState);
    }

    public void Logout() {
        ChangeState(false);
        _loginManager.Logout();
    }
}
