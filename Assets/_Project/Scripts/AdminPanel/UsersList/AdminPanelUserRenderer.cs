using TMPro;
using UnityEngine;

public class AdminPanelUserRenderer : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _emailText;
    [SerializeField] private TextMeshProUGUI _roleText;
    [SerializeField] private string _adminRole;
    [SerializeField] private string _userRole;

    private UserData _userData;
    private AdminPanelUsersList _usersList;

    public UserData UserData => _userData;

    public void SetUserData(UserData userData) {
        _userData = userData;
        _nameText.text = userData.Name;
        _emailText.text = userData.Email;

        if (userData.UserType == UserType.Admin)
            _roleText.text = _adminRole;
        else
            _roleText.text = _userRole;
    }

    public void DeleteUser() {
        _usersList.DeleteUser(this);
    }

    public void SetUsersList(AdminPanelUsersList usersList) {
        _usersList = usersList;
    }
}
