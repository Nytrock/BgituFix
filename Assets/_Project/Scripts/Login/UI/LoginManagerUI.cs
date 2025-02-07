using TMPro;
using UnityEngine;

public class LoginManagerUI : MonoBehaviour {
    [SerializeField] private LoginManager _loginManager;
    [SerializeField] private GameObject _panel;
    [SerializeField] private LoginErrorMessageUI _errorMessage;
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _passwordInput;

    private void Awake() {
        _loginManager.LoginStateChanged += ChangeState;
        _loginManager.LoginError += _errorMessage.ShowLoginError;
    }

    private void ChangeState(bool newState) {
        _panel.SetActive(newState);
        if (newState)
            _errorMessage.ChangeState(false);
    }

    public void Login() {
        string name = _nameInput.text;
        string password = _passwordInput.text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password)) {
            _errorMessage.ShowEmptyFieldsError();
            return;
        }

        UserLoginData loginData = new(name, password);
        StartCoroutine(_loginManager.TryLogin(loginData));
    }
}
