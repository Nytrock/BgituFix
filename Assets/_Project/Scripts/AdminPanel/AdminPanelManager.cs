using UnityEngine;

public class AdminPanelManager : StateMachine {
    [SerializeField] private UserManager _userManager;
    [SerializeField] private AdminPanelAddUser _userForm;
    [SerializeField] private AdminPanelStatistics _stats;
    [SerializeField] private AdminPanelUsersList _usersList;

    private void Awake() {
        _userManager.ClientSetuped += Setup;
        ChangeState(false);
    }

    public override void ChangeState(bool newState) {
        base.ChangeState(newState);
        if (newState)
            ChangeUserFormState(false);
    }

    private void Setup() {
        if (_userManager.ClientType != UserType.Admin)
            return;

        _stats.Setup();
        _usersList.Setup(_userManager);
    }

    public void CreateUser(UserData userData) {
        _usersList.AddUser(userData);
        _stats.AddUser();
        ChangeUserFormState(false);
        StartCoroutine(_userManager.CreateUser(userData));
    }

    public void ChangeUserFormState(bool newState) {
        _userForm.ChangeState(newState);
    }
}
