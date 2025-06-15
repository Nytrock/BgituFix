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

        if (name.Length < 8 || password.Length < 8)
            return;

        string email = _emailInput.text;
        bool isAdmin = _isAdminToggle.isOn;
        UserData userData = new(name, password, email, isAdmin);
        _adminManager.CreateUser(userData);
    }
}
