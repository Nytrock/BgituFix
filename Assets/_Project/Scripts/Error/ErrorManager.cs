using EvtSource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;

public class ErrorManager : MonoBehaviour {
    [SerializeField] private TokenManager _tokenManager;
    [SerializeField] private MapAudienceManager _audienceManager;
    [SerializeField] private UserManager _userManager;
    [SerializeField] private string _APIPathForErrors;
    [SerializeField] private string _APIPathToSSE;

    [SerializeField] private ErrorManagerData _data;
    private MapData _mapData;

    public IEnumerable<ComputerErrorData> Errors => _data.Errors;

    public event Action ErrorsLoaded;
    public event Action ErrorsUpdated;

    public event Action<ComputerErrorData> ErrorAdded;
    public event Action<ComputerErrorData> ErrorChanged;
    public event Action<ComputerErrorData> ErrorDeleted;

    private void Awake() {
        _tokenManager.TokenLoaded += delegate { SSESetup(); };
    }

    public IEnumerator GetErrors(MapData mapData) {
        _mapData = mapData;

        UnityWebRequest request = APIUtility.Get(_APIPathForErrors);
        yield return request.SendWebRequestSafely();
        _data = request.ToData<ErrorManagerData>();

        GenerateErrors();
        ErrorsLoaded?.Invoke();
    }

    private void GenerateErrors() {
        foreach (var error in _data.Errors)
            AddError(error);
    }

    private void SSESetup() {
        Uri uri = new(APIUtility.API_URL + _APIPathToSSE);
        HttpClient client = new();
        client.SetToken();
        client.SetHeaders();

        EventSourceReader evt = new EventSourceReader(uri, client).Start();
        evt.MessageReceived += (object sender, EventSourceMessageEventArgs e) => {
            ErrorManagerData newData = e.ToData<ErrorManagerData>();
            CheckNewData(newData);
        };

        evt.Disconnected += (object sender, DisconnectEventArgs e) => {
            if (!Application.isPlaying)
                return;

            if (e.Exception.Message.Contains("Not Found")) {
                Debug.Log($"Ошибка, SSE сервис не найден");
            } else {
                Debug.Log($"Переподключение: {e.ReconnectDelay} - Ошибка: {e.Exception}");
                evt.Start();
            }
        };
    }

    private void CheckNewData(ErrorManagerData newData) {
        foreach (var newError in newData.Errors) {
            if (!_data.Contains(newError)) {
                AddError(newError);
            } else {
                ComputerErrorData oldError = _data.GetErrorById(newError.Id);
                if (!oldError.Equals(newError))
                    ChangeError(oldError, newError.IsSolved);
            }
        }

        List<ComputerErrorData> errorsToDelete = new();
        foreach (var oldError in _data.Errors)
            if (!newData.Contains(oldError))
                errorsToDelete.Add(oldError);

        foreach (var error in errorsToDelete)
            DeleteError(error);
    }

    public IEnumerator ChangeErrorSolveInDatabase(ComputerErrorData error, bool isSolved) {
        BoolChangeData dataToSend = new(error.Id, isSolved);
        UnityWebRequest request = APIUtility.Put(_APIPathForErrors, dataToSend, error.Id);
        yield return request.SendWebRequestSafely();
    }

    public IEnumerator DeleteErrorInDatabase(ComputerErrorData error) {
        UnityWebRequest request = APIUtility.Delete(_APIPathForErrors, error.Id);
        yield return request.SendWebRequestSafely();
    }

    public IEnumerator CreateErrorInDatabase(ComputerErrorType type, string comment, int computerId) {
        int clientId = _userManager.ClientId;
        ComputerErrorData newError = new(computerId, clientId, type, comment);
        UnityWebRequest request = APIUtility.Post(_APIPathForErrors, newError);
        yield return request.SendWebRequestSafely();
    }

    private void AddError(ComputerErrorData newError) {
        ComputerData computerData = _mapData.GetComputerById(newError.ComputerId);
        newError.SetAudienceId(computerData.AudienceId);
        _data.AddError(newError);

        ErrorAdded?.Invoke(newError);
        ErrorsUpdated?.Invoke();
    }

    private void DeleteError(ComputerErrorData error) {
        _data.DeleteError(error);

        ErrorDeleted?.Invoke(error);
        ErrorsUpdated?.Invoke();
    }

    private void ChangeError(ComputerErrorData error, bool isSolved) {
        error.ChangeSolved(isSolved);

        ErrorChanged?.Invoke(error);
        ErrorsUpdated?.Invoke();
    }

    public void DeleteErrorsByComputerId(ComputerData data) {
        List<ComputerErrorData> errorsToDelete = new();
        foreach (var error in _data.Errors)
            if (error.ComputerId == data.Id)
                errorsToDelete.Add(error);

        foreach (var error in errorsToDelete)
            DeleteError(error);
    }
}
