using System;
using UnityEngine;

[Serializable]
public class UserData {
    [SerializeField] private int id;
    [SerializeField] private string username;
    [SerializeField] private string email;
    [SerializeField] private string role;
    [SerializeField] private string sub;

    private UserType _userType;

    public int Id => id;
    public string Name => username;
    public UserType UserType => _userType;

    public UserData() {
        username = "Not found";
        _userType = UserType.None;
    }

    public void SetupClient() {
        username = sub;
        if (role == "ROLE_USER")
            _userType = UserType.User;
        else if (role == "ROLE_ADMIN")
            _userType = UserType.Admin;
        else
            _userType = UserType.None;
    }
}
