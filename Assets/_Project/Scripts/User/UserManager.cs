using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class UserManager : MonoBehaviour {
    [SerializeField] private LoginManager _loginManager;
    [SerializeField] private ProfileManager _profileManager;
    [SerializeField] private string _APIPathToCreateUser;
    [SerializeField] private string _APIPathToGetUsers;

    [SerializeField] private UserData _clientData;
    [SerializeField] private UserManagerData _usersData;

    public UserType ClientType => _clientData.UserType;
    public int ClientId => _clientData.Id;
    public int UsersCount => _usersData.Users.Count();
    public IEnumerable<UserData> Users => _usersData.Users;

    public event Action ClientSetuped;

    private void Awake() {
        _loginManager.TokenLoaded += delegate { StartCoroutine(UpdateUserData()); };
    }

    public UserData GetUserDataById(int userId) {
        foreach (var user in _usersData.Users)
            if (user.Id == userId)
                return user;
        return null;
    }

    private IEnumerator UpdateUserData() {
        string token = _loginManager.Token;

        try {
            string tokenContentEncoded = token.Split('.')[1];
            tokenContentEncoded = tokenContentEncoded.Replace('_', '/').Replace('-', '+');
            byte[] contentBytes = Convert.FromBase64String(tokenContentEncoded);
            string content = Encoding.UTF8.GetString(contentBytes);

            _clientData = JsonUtility.FromJson<UserData>(content);
            _clientData.SetupClient();
            _profileManager.SetupProfile(_clientData);
        } catch {
            _clientData = new();
            yield break;
        } finally {
            ClientSetuped?.Invoke();
        }

        UnityWebRequest request = RequestUtility.APIGet(_APIPathToGetUsers, token);
        yield return request.SendWebRequest();
        _usersData = request.ToData<UserManagerData>();
    }

    public IEnumerator CreateUser(NewUserData newUser) {
        string token = _loginManager.Token;
        UnityWebRequest request = RequestUtility.APIPost(_APIPathToCreateUser, newUser, token);
        yield return request.SendWebRequest();
    }
}
