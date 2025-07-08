using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminPanelAddUser : MonoBehaviour {
    [SerializeField] private AdminPanelManager _adminManager;
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _emailInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private Toggle _isAdminToggle;

    public void TryToAddUser() {
        string name = _nameInput.text;
        string password = _passwordInput.text;

        if (password.Length < 8)
            return;

        string email = _emailInput.text;
        bool isAdmin = _isAdminToggle.isOn;
        UserData userData = new(name, password, email, isAdmin);
        _adminManager.CreateUser(userData);
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
        if (newState)
            ClearForm();
    }

    private void ClearForm() {
        _nameInput.text = string.Empty;
        _emailInput.text = string.Empty;
        _passwordInput.text = string.Empty;
        _isAdminToggle.isOn = false;
    }
}
