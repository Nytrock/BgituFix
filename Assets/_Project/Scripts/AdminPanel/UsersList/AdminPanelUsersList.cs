using UnityEngine;

public class AdminPanelUsersList : MonoBehaviour {
    [SerializeField] private AdminPanelUserRendererPool _pool;

    private UserManager _userManager;

    public void Setup(UserManager userManager) {
        _userManager = userManager;
        foreach (var user in _userManager.Users)
            AddUser(user);
    }

    public void AddUser(NewUserData newUserData) {

    }

    private void AddUser(UserData userData) {

    }
}
