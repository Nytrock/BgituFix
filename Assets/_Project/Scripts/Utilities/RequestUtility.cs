using UnityEngine;
using UnityEngine.Networking;

public static class RequestUtility {
    // Temp
    private const string API_URL = "";

    public static UnityWebRequest GetFromAPI(string path) {
        return UnityWebRequest.Get(API_URL + path);
    }

    public static T ToData<T>(this UnityWebRequest result) {
        return JsonUtility.FromJson<T>(result.downloadHandler.text);
    }

    public static string ToJson<T>(T data) {
        return JsonUtility.ToJson(data);
    }
}
