using System;
using UnityEngine;

[Serializable]
public class UserData {
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private bool _isAuthorized;
    [SerializeField] private bool _isAdmin;
    private UserType _type;

    public int Id => _id;
    public string Name => _name;
    public UserType Type => _type;

    public UserData() {
        _name = "Not found";
        _isAdmin = false;
        _isAuthorized = false;
    }

    public void SetupType() {
        if (!_isAuthorized)
            _type = UserType.None;
        else if (!_isAdmin)
            _type = UserType.Teacher;
        else
            _type = UserType.Admin;
    }
}
