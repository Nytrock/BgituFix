using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class UserManager : MonoBehaviour {
    [SerializeField] private UrlManager _urlManager;
    [SerializeField] private string _APIPathToGetClient;
    [SerializeField] private string _APIPathToGetUsers;

    [SerializeField] private UserData _clientData;
    [SerializeField] private List<UserData> _usersData;

    public UserType ClientType => _clientData.Type;
    public int ClientId => _clientData.Id;

    public void Awake() {
        StartCoroutine(UpdateUserData());
    }

    public UserData GetUserDataById(int userId) {
        foreach (var user in _usersData)
            if (user.Id == userId)
                return user;
        return null;
    }

    private IEnumerator UpdateUserData() {
        if (string.IsNullOrEmpty(_APIPathToGetClient)) {
            _clientData.SetupType();
            yield break;
        }

        string token = _urlManager.GetParameter("token");
        try {
            string tokenContentEncoded = token.Split('.')[1];
            byte[] contentBytes = Convert.FromBase64String(tokenContentEncoded);
            string content = Encoding.UTF8.GetString(contentBytes);
            _clientData = JsonUtility.FromJson<UserData>(content);
        } catch {
            _clientData = new();
            yield break;
        }

        UnityWebRequest request = RequestUtility.APIGet(_APIPathToGetUsers, token);
        yield return request.SendWebRequest();
        _usersData = request.ToData<List<UserData>>();
    }
}
