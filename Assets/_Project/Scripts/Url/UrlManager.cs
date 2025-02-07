using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UrlManager : MonoBehaviour {
    [SerializeField] private string _APIPathToCheckToken;
    [SerializeField] private string _editorUrl;

    private string _url;
    private string _token;

    private Dictionary<string, string> _parameters;

    public string Token => _token;

    public event Action TokenGetted;

    private void Start() {
        if (Application.isEditor)
            _url = _editorUrl;
        else
            _url = Application.absoluteURL;

        GetDataFromUrl();
        StartCoroutine(CheckTokenValid());
    }

    private void GetDataFromUrl() {
        _parameters = new();
        if (string.IsNullOrEmpty(_url) || _url.Split('?').Length != 2) {
            _token = string.Empty;
            return;
        }

        string[] parameters = _url.Split('?')[1].Split('&');
        foreach (var parameter in parameters) {
            int index = parameter.IndexOf('=');
            if (index > 0)
                _parameters[parameter[..index]] = parameter[(index + 1)..];
        }

        _token = _parameters.GetValueOrDefault("token", string.Empty);
    }

    private IEnumerator CheckTokenValid() {
        if (_token == string.Empty) {
            TokenGetted?.Invoke();
            yield break;
        }

        UnityWebRequest request = RequestUtility.APIGet(_APIPathToCheckToken, _token);
        yield return request.SendWebRequest();
        ValidationData validationData = request.ToData<ValidationData>();

        if (!validationData.IsValid)
            _token = string.Empty;
        TokenGetted?.Invoke();
    }
}
