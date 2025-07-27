using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour {
    [SerializeField] private string _APIPathToLogin;

    public event Action<bool> LoginStateChanged;
    public event Action LoginError;
    public static event Action OnLogout;

    public IEnumerator TryLogin(UserLoginData loginData) {
        UnityWebRequest request = APIUtility.Post(_APIPathToLogin, loginData);
        yield return request.SendWebRequestNotSafely();
        TokenData tokenData = request.ToData<TokenData>();

        if (request.responseCode == 401) {
            LoginError?.Invoke();
            yield break;
        }

        APIUtility.UpdateTokenData(tokenData);
        LoginStateChanged?.Invoke(false);
    }

    public static void Logout() {
        OnLogout?.Invoke();
        OnLogout = null;

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void ChangeLoginState(bool newState) {
        LoginStateChanged?.Invoke(newState);
    }
}
