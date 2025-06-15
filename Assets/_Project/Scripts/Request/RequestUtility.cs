using EvtSource;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;

public static class RequestUtility {
    public const string API_URL = "https://bgitusec.online/";
    private const string TOKEN_HEADER = "Authorization";
    private const string TOKEN_TEXT = "Bearer ";


    public static UnityWebRequest APIGet(string path, string token = "") {
        UnityWebRequest request = UnityWebRequest.Get(API_URL + path);
        if (token != string.Empty)
            SetToken(request, token);
        SetHeaders(request);
        return request;
    }

    public static UnityWebRequest APIPost<T>(string path, T data, string token = "") {
        UnityWebRequest request = UnityWebRequest.Post(API_URL + path, data.ToJson(), "json");
        if (token != string.Empty)
            SetToken(request, token);
        SetHeaders(request);
        return request;
    }

    public static UnityWebRequest APIPut<T>(string path, T data, string token = "") {
        string dataToPut = ToJson(data);
        string apiPath = API_URL + path;

        UnityWebRequest request = UnityWebRequest.Put(apiPath, dataToPut);
        if (token != string.Empty)
            SetToken(request, token);
        SetHeaders(request);
        return request;
    }

    public static UnityWebRequest APIDelete(string path, int id, string token = "") {
        string apiPath = API_URL + path + '/' + id.ToString();
        UnityWebRequest request = UnityWebRequest.Delete(apiPath);
        if (token != string.Empty)
            SetToken(request, token);
        SetHeaders(request);
        return request;
    }

    private static void SetToken(UnityWebRequest request, string token) {
        request.SetRequestHeader(TOKEN_HEADER, TOKEN_TEXT + token);
    }

    public static void SetToken(this HttpClient client, string token) {
        client.DefaultRequestHeaders.Add(TOKEN_HEADER, TOKEN_TEXT + token);
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
}
