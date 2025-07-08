using System;
using UnityEngine;

public class TokenManager : MonoBehaviour {
    [SerializeField] private string _fileName;
    [SerializeField] private LoginManager _loginManager;

    private static FileManager _fileManager;

    public event Action TokenLoaded;

    private void Awake() {
        APIUtility.TokenUpdated += SaveToken;
        LoginManager.OnLogout += DeleteToken;
    }

    private void Start() {
        _fileManager = new(_fileName);
        TryLoadTokenFromSave();
    }

    private void TryLoadTokenFromSave() {
        TokenData tokenData = _fileManager.Load<TokenData>();

        _loginManager.ChangeLoginState(tokenData == null);
        if (tokenData == null) {
            _loginManager.LoginStateChanged += CheckLogin;
            return;
        }

        APIUtility.SetTokenData(tokenData);
        APIUtility.TokenUpdated += InitialTokenUpdated;
        StartCoroutine(APIUtility.UpdateToken());
    }

    private void InitialTokenUpdated(TokenData data) {
        APIUtility.TokenUpdated -= InitialTokenUpdated;
        TokenLoaded?.Invoke();
    }

    private void CheckLogin(bool isLogin) {
        if (isLogin)
            return;

        _loginManager.LoginStateChanged -= CheckLogin;
        TokenLoaded?.Invoke();
    }

    private void SaveToken(TokenData data) {
        _fileManager.Save(data);
    }

    private void DeleteToken() {
        _fileManager.Delete();
    }
}
