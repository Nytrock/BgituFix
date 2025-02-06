using System;
using System.Collections.Generic;
using UnityEngine;

public class UrlManager : MonoBehaviour {
    [SerializeField] private string _editorUrl;
    private string _url;

    private Dictionary<string, string> _parameters;

    public string Token {
        get {
            GetDataFromUrl();
            return _parameters.GetValueOrDefault("token", string.Empty);
        }
    }

    public event Action TokenGetted;

    private void Awake() {
        if (Application.isEditor)
            _url = _editorUrl;
        else
            _url = Application.absoluteURL;

        GetDataFromUrl();
        TokenGetted?.Invoke();
    }

    private void GetDataFromUrl() {
        _parameters = new();
        if (string.IsNullOrEmpty(_url) || _url.Split('?').Length != 2)
            return;

        string[] parameters = _url.Split('?')[1].Split('&');
        foreach (var parameter in parameters) {
            int index = parameter.IndexOf('=');
            if (index > 0)
                _parameters[parameter[..index]] = parameter[(index + 1)..];
        }
    }
}
