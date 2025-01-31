using System;
using UnityEngine;

[Serializable]
public class UserData {
    [SerializeField] private string _name;
    [SerializeField] private bool _isAuthorized;
    [SerializeField] private bool _isAdmin;

    public string Name => _name;
    public bool IsAuthorized => _isAuthorized;
    public bool IsAdmin => _isAdmin;

    public UserData() {
        _name = "Not found";
        _isAdmin = false;
        _isAuthorized = false;
    }
}
