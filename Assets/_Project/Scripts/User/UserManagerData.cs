using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserManagerData {
    [SerializeField] private List<UserData> users;

    public IEnumerable<UserData> Users => users;
}
