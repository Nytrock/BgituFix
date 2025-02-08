using UnityEngine;

public class AdminPanelUsersList : MonoBehaviour {
    [SerializeField] private AdminPanelUserRendererPool _pool;

    private UserManager _userManager;

    public void Setup(UserManager userManager) {
        _userManager = userManager;
        _pool.SetUsersList(this);
        _userManager.UsersGetted += GenerateUsers;
    }

    private void GenerateUsers() {
        foreach (var user in _userManager.Users)
            if (user.Id != _userManager.ClientId)
                AddUser(user);
    }

    public void AddUser(UserData userData) {
        AdminPanelUserRenderer renderer = _pool.GetObject();
        renderer.SetUserData(userData);
    }

    public void DeleteUser(AdminPanelUserRenderer renderer) {
        _pool.PutObject(renderer);
        StartCoroutine(_userManager.DeleteUser(renderer.UserData));
    }
}
