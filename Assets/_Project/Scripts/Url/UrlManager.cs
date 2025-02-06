using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UrlManager : MonoBehaviour {
    [SerializeField] private string _editorUrl;
    [SerializeField] private string _APIPathToLogin;

    private Dictionary<string, string> _parameters;
    private string _token;

    public string Token => _token;

    public event Action TokenGetted;

    private void Awake() {
        if (Application.isEditor) {
            GetDataFromUrl(_editorUrl);
            return;
        }

        GetDataFromUrl(Application.absoluteURL);
    }

    private void GetDataFromUrl(string url) {
        _parameters = new(); ;
        if (string.IsNullOrEmpty(url) || url.Split('?').Length != 2)
            return;

        string[] parameters = url.Split('?')[1].Split('&');
        foreach (var parameter in parameters) {
            int index = parameter.IndexOf('=');
            if (index > 0)
                _parameters[parameter[..index]] = parameter[(index + 1)..];
        }

        StartCoroutine(GetTokenFromData());
    }

    private IEnumerator GetTokenFromData() {
        string username = _parameters.GetValueOrDefault("username", null);
        string password = _parameters.GetValueOrDefault("password", null);
        UserLoginData loginData = new(username, password);

        UnityWebRequest request = RequestUtility.APIPost(_APIPathToLogin, loginData);
        yield return request.SendWebRequest();

        TokenData tokenData = request.ToData<TokenData>();
        _token = tokenData.Token;

        TokenGetted?.Invoke();
    }
}
