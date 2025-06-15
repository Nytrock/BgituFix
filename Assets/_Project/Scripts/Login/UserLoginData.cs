using System;
using UnityEngine;

[Serializable]
public class UserLoginData {
    [SerializeField] private string username;
    [SerializeField] private string password;

    public UserLoginData(string username, string password) {
        this.username = username;
        this.password = password;
    }
}
