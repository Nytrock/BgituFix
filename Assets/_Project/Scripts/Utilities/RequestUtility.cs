using UnityEngine;
using UnityEngine.Networking;

public static class RequestUtility {
    // Temp
    private const string API_URL = "";
    private const string TOKEN_HEADER = "Authorization";
    private const string TOKEN_TEXT = "Bearer ";


    public static UnityWebRequest APIGet(string path, string token) {
        UnityWebRequest request = UnityWebRequest.Get(API_URL + path);
        SetToken(request, token);
        return request;
    }

    public static UnityWebRequest APIPost(string path, WWWForm form, string token) {
        UnityWebRequest request = UnityWebRequest.Post(API_URL + path, form);
        SetToken(request, token);
        return request;
    }

    public static UnityWebRequest APIPut<T>(string path, int id, T data, string token) {
        string dataToPut = ToJson(data);
        string apiPath = API_URL + path + id.ToString();

        UnityWebRequest request = UnityWebRequest.Put(apiPath, dataToPut);
        SetToken(request, token);
        return request;
    }

    public static UnityWebRequest APIDelete(string path, int id, string token) {
        string apiPath = API_URL + path + id.ToString();
        UnityWebRequest request = UnityWebRequest.Delete(apiPath);
        SetToken(request, token);
        return request;
    }

    private static void SetToken(UnityWebRequest request, string token) {
        request.SetRequestHeader(TOKEN_HEADER, TOKEN_TEXT + token);
    }

    public static T ToData<T>(this UnityWebRequest result) {
        return JsonUtility.FromJson<T>(result.downloadHandler.text);
    }

    public static string ToJson<T>(T data) {
        return JsonUtility.ToJson(data);
    }
}
