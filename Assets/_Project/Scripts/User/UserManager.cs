using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class UserManager : MonoBehaviour {
    [SerializeField] private TokenManager _tokenManager;
    [SerializeField] private ProfileManager _profileManager;
    [SerializeField] private string _APIPathToCreateUser;
    [SerializeField] private string _APIPathForUsers;

    [SerializeField] private UserData _clientData;
    [SerializeField] private UserManagerData _usersData;

    public UserType ClientType => _clientData.UserType;
    public int ClientId => _clientData.Id;
    public int UsersCount => _usersData.Users.Count();
    public IEnumerable<UserData> Users => _usersData.Users;

    public event Action ClientSetuped;
    public event Action UsersGetted;
    public event Action UsersCountChanged;

    private void Awake() {
        _tokenManager.TokenLoaded += delegate { StartCoroutine(UpdateUserData()); };
    }

    public UserData GetUserDataById(int userId) {
        foreach (var user in _usersData.Users)
            if (user.Id == userId)
                return user;
        return null;
    }

    private IEnumerator UpdateUserData() {
        try {
            string tokenContentEncoded = APIUtility.Token.Split('.')[1];
            string tokenBase64 = tokenContentEncoded.Replace('_', '/').Replace('-', '+');
            switch (tokenBase64.Length % 4) {
                case 2: tokenBase64 += "=="; break;
                case 3: tokenBase64 += "="; break;
            }
            byte[] contentBytes = Convert.FromBase64String(tokenBase64);
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

        UnityWebRequest request = APIUtility.Get(_APIPathForUsers);
        yield return request.SendWebRequestSafely();
        _usersData = request.ToData<UserManagerData>();
        _usersData.SetupUsers();
        UsersGetted?.Invoke();
    }

    public IEnumerator CreateUser(UserData newUser) {
        UnityWebRequest request = APIUtility.Post(_APIPathToCreateUser, newUser);
        yield return request.SendWebRequestSafely();
        IdData idData = request.ToData<IdData>();

        newUser.SetId(idData.Id);
        _usersData.AddUser(newUser);
        UsersCountChanged?.Invoke();
    }

    public IEnumerator DeleteUser(UserData userData) {
        UnityWebRequest request = APIUtility.Delete(_APIPathForUsers, userData.Id);
        yield return request.SendWebRequestSafely();

        _usersData.DeleteUser(userData);
        UsersCountChanged?.Invoke();
    }
}
