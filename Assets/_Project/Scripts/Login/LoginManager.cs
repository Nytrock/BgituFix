using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour {
    [SerializeField] private string _APIPathToLogin;
    [SerializeField] private string _APIPathToCheckTokenValid;
    [SerializeField] private string _fileName;

    private SaveFileManager<TokenData> _fileManager;
    private TokenData _tokenData;

    public string Token => _tokenData.Token;

    public event Action<bool> LoginStateChanged;
    public event Action LoginError;
    public event Action TokenLoaded;

    private void Start() {
        _fileManager = new(_fileName);
        TryLoadTokenFromSave();
    }

    private void TryLoadTokenFromSave() {
        _tokenData = _fileManager.Load();

        if (_tokenData == null) {
            LoginStateChanged?.Invoke(true);
            return;
        }

        StartCoroutine(CheckTokenValid());
    }

    private IEnumerator CheckTokenValid() {
        UnityWebRequest request = RequestUtility.APIGet(_APIPathToCheckTokenValid, _tokenData.Token);
        yield return request.SendWebRequest();
        ValidationData validationData = request.ToData<ValidationData>();

        if (!validationData.IsValid) {
            LoginStateChanged?.Invoke(true);
            yield break;
        }

        LoginStateChanged?.Invoke(false);
        TokenLoaded?.Invoke();
    }

    public IEnumerator TryLogin(UserLoginData loginData) {
        UnityWebRequest request = RequestUtility.APIPost(_APIPathToLogin, loginData);
        yield return request.SendWebRequest();
        TokenData tokenData = request.ToData<TokenData>();

        if (tokenData == null) {
            LoginError?.Invoke();
            yield break;
        }

        _tokenData = tokenData;
        _fileManager.Save(_tokenData);

        LoginStateChanged?.Invoke(false);
        TokenLoaded?.Invoke();
    }

    public void Logout() {
        _fileManager.Delete();

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
