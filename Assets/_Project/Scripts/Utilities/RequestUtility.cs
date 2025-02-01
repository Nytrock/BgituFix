using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class RequestUtility {
    // Temp
    private const string API_URL = "";

    public static UnityWebRequest GetFromAPI(string path) {
        return UnityWebRequest.Get(API_URL + path);
    }

    public static void LoadDataToSend(this UnityWebRequest www, string dataToSend) {
        www.SetRequestHeader("Content-Type", "application/json");
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(dataToSend));
    }

    public static T ToData<T>(this UnityWebRequest result) {
        return JsonUtility.FromJson<T>(result.downloadHandler.text);
    }

    public static string ToJson<T>(T data) {
        return JsonUtility.ToJson(data);
    }
}
