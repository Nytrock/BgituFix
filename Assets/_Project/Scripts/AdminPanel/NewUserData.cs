using System;
using UnityEngine;

[Serializable]
public class NewUserData {
    [SerializeField] private string username;
    [SerializeField] private string password;
    [SerializeField] private string email;
    [SerializeField] private string role;

    public string Username => username;
    public string Password => password;
    public string Email => email;
    public string Role => role;

    public NewUserData(string username, string password, string email, bool isAdmin) {
        this.username = username;
        this.password = password;
        this.email = email;

        if (isAdmin)
            role = "ROLE_ADMIN";
        else
            role = "ROLE_USER";
    }
}
