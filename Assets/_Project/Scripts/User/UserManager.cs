using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class UserManager : MonoBehaviour {
    [SerializeField] private UrlManager _urlManager;
    [SerializeField] private string _APIPathToGetUsers;

    [SerializeField] private UserData _clientData;
    [SerializeField] private UserManagerData _usersData;

    public UserType ClientType => _clientData.UserType;
    public int ClientId => _clientData.Id;

    public event Action ClientSetuped;

    private void Awake() {
        _urlManager.TokenGetted += delegate { StartCoroutine(UpdateUserData()); };
    }

    public UserData GetUserDataById(int userId) {
        foreach (var user in _usersData.Users)
            if (user.Id == userId)
                return user;
        return null;
    }

    private IEnumerator UpdateUserData() {
        string token = _urlManager.Token;

        try {
            string tokenContentEncoded = token.Split('.')[1];
            tokenContentEncoded = tokenContentEncoded.Replace('_', '/').Replace('-', '+');
            byte[] contentBytes = Convert.FromBase64String(tokenContentEncoded);
            string content = Encoding.UTF8.GetString(contentBytes);
            _clientData = JsonUtility.FromJson<UserData>(content);
            _clientData.SetupClient();
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
}
