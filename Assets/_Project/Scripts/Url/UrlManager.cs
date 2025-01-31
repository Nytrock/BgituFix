using System.Collections.Generic;
using UnityEngine;

public class UrlManager : MonoBehaviour {
    [SerializeField] private string _editorUrl;
    private Dictionary<string, string> _parameters;

    private void GetDataFromUrl() {
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
    }

    public string GetParameter(string parameteer) {
        GetDataFromUrl();
        return _parameters.GetValueOrDefault(parameteer, null);
    }
}
