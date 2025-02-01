using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UserManager : MonoBehaviour {
    [SerializeField] private UrlManager _urlManager;
    [SerializeField] private string _APIPathToGetUser;
    [SerializeField] private UserData _data;

    public void Awake() {
        StartCoroutine(UpdateUserData());
    }

    private IEnumerator UpdateUserData() {
        if (string.IsNullOrEmpty(_APIPathToGetUser))
            yield break;

        string token = _urlManager.GetParameter("token");
        if (token is null) {
            _data = new();
            yield break;
        }

        UnityWebRequest www = RequestUtility.GetFromAPI(_APIPathToGetUser);
        www.LoadDataToSend($"{{\"token\": \"{token}\"}}");
        yield return www.SendWebRequest();

        _data = www.ToData<UserData>();
    }

    public UserType GetUserType() {
        StartCoroutine(UpdateUserData());

        if (!_data.IsAuthorized)
            return UserType.None;
        else if (!_data.IsAdmin)
            return UserType.Teacher;
        else
            return UserType.Admin;
    }
}
