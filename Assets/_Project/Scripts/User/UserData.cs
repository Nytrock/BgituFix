using System;
using UnityEngine;

[Serializable]
public class UserData {
    [SerializeField] private int id;
    [SerializeField] private int sub;
    [SerializeField] private string name;
    [SerializeField] private string email;
    [SerializeField] private string role;
    [SerializeField] private string password;
    [SerializeField] private bool sentNotifications;

    private UserType _userType;

    public int Id => id;
    public string Name => name;
    public string Email => email;
    public UserType UserType => _userType;

    public UserData() {
        name = "Not found";
        _userType = UserType.None;
    }

    public UserData(string name, string password, string email, bool isAdmin) {
        id = -1;
        this.name = name;
        this.email = email;
        this.password = password;

        if (isAdmin) {
            role = "ROLE_ADMIN";
            _userType = UserType.Admin;
        } else {
            role = "ROLE_USER";
            _userType = UserType.User;
        }
    }

    public void SetupClient() {
        id = sub;
        SetupRole();
    }

    public void SetupRole() {
        if (role == "ROLE_USER")
            _userType = UserType.User;
        else if (role == "ROLE_ADMIN")
            _userType = UserType.Admin;
        else
            _userType = UserType.None;
    }

    public void SetId(int id) {
        if (this.id != -1)
            return;

        this.id = id;
    }
}
