using UnityEngine;

public class ProfileManager : MonoBehaviour {
    [SerializeField] private GameObject _panel;
    [SerializeField] private LoginManager _loginManager;
    [SerializeField] private ErrorManager _errorManager;
    [SerializeField] private ProfileInfoRenderer _infoRenderer;
    [SerializeField] private ProfileErrorsRenderer _errorsRenderer;

    public void SetupProfile(UserData clientData) {
        _infoRenderer.SetInfo(clientData, _errorManager);
        _errorsRenderer.Setup(clientData, _errorManager);
    }
}
