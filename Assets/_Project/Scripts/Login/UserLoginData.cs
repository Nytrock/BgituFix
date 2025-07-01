using System;
using UnityEngine;

[Serializable]
public class UserLoginData {
    [SerializeField] private string name;
    [SerializeField] private string password;

    public UserLoginData(string name, string password) {
        this.name = name;
        this.password = password;
    }
}
