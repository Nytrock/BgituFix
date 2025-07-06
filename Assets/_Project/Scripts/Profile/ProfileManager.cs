using UnityEngine;

public class ProfileManager : StateMachine {
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
}
