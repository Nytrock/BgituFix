using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserManagerData {
    [SerializeField] private List<UserData> users;

    public IEnumerable<UserData> Users => users;

    public void SetupUsers() {
        foreach (var user in users)
            user.SetupRole();
    }

    public void AddUser(UserData newUser) {
        users.Add(newUser);
    }

    public void DeleteUser(UserData userData) {
        users.Remove(userData);
    }
}
