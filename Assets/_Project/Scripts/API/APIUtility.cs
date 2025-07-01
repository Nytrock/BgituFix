using EvtSource;
using System;
using System.Collections;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;

public static class APIUtility {
    public const string API_URL = "https://bgitusec.online:7111/api";
    public const string TOKEN_UPDATE_PATH = "/auth/refresh-token";
    private const string TOKEN_HEADER = "Authorization";
    private const string TOKEN_TEXT = "Bearer ";

    private static TokenData _tokenData;

    public static string Token => _tokenData.Token;

    public static event Action<TokenData> TokenUpdated;

    public static UnityWebRequest Get(string path) {
        UnityWebRequest request = UnityWebRequest.Get($"{API_URL}/{path}");
        SetupRequestBeforeSend(request);
        return request;
    }

    public static UnityWebRequest Post<T>(string path, T data) {
        UnityWebRequest request = UnityWebRequest.Post($"{API_URL}/{path}", data.ToJson(), "json");
        SetupRequestBeforeSend(request);
        return request;
    }

    public static UnityWebRequest Put<T>(string path, T data, int id) {
        UnityWebRequest request = UnityWebRequest.Put($"{API_URL}/{path}/{id}", data.ToJson());
        SetupRequestBeforeSend(request);
        return request;
    }

    public static UnityWebRequest Delete(string path, int id) {
        UnityWebRequest request = UnityWebRequest.Delete($"{API_URL}/{path}/{id}");
        SetupRequestBeforeSend(request);
        return request;
    }

    public static IEnumerator SendWebRequestSafely(this UnityWebRequest request) {
        yield return request.SendWebRequest();
        if (request.responseCode != 400)
            yield break;

        Debug.Log(2);
        yield return UpdateToken();
        yield return request.SendWebRequest();
    }

    private static void SetupRequestBeforeSend(UnityWebRequest request) {
        if (_tokenData is not null)
            request.SetRequestHeader(TOKEN_HEADER, TOKEN_TEXT + _tokenData.Token);
        SetHeaders(request);
    }

    public static IEnumerator UpdateToken() {
        UnityWebRequest request = UnityWebRequest.Post(API_URL + TOKEN_UPDATE_PATH, _tokenData.ToJson(), "json");
        SetHeaders(request);
        yield return request.SendWebRequest();

        if (request.responseCode == 200) {
            _tokenData = request.ToData<TokenData>();
            TokenUpdated?.Invoke(_tokenData);
        } else {
            LoginManager.Logout();
        }
    }

    public static void SetToken(this HttpClient client) {
        client.DefaultRequestHeaders.Add(TOKEN_HEADER, TOKEN_TEXT + _tokenData.Token);
    }

    private static void SetHeaders(UnityWebRequest request) {
        request.SetRequestHeader("Content-Type", "application/json");
    }

    public static void SetHeaders(this HttpClient client) {
        client.DefaultRequestHeaders.Accept.Add(new("application/json"));
    }

    public static T ToData<T>(this UnityWebRequest result) {
        return JsonUtility.FromJson<T>(result.downloadHandler.text);
    }

    public static T ToData<T>(this EventSourceMessageEventArgs result) {
        return JsonUtility.FromJson<T>(result.Message);
    }

    public static string ToJson<T>(this T data) {
        return JsonUtility.ToJson(data);
    }

    public static void SetTokenData(TokenData tokenData) {
        _tokenData = tokenData;
    }

    public static void UpdateTokenData(TokenData tokenData) {
        _tokenData = tokenData;
        TokenUpdated?.Invoke(_tokenData);
    }
}
