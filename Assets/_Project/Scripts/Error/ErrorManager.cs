using EvtSource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;

public class ErrorManager : MonoBehaviour {
    [SerializeField] private UrlManager _urlManager;
    [SerializeField] private MapAudienceManager _audienceManager;
    [SerializeField] private UserManager _userManager;
    [SerializeField] private string _APIPathToGetErrors;
    [SerializeField] private string _APIPathToChangeError;
    [SerializeField] private string _APIPathToDeleteError;
    [SerializeField] private string _APIPathToAddError;
    [SerializeField] private string _APIPathToSSE;

    [SerializeField] private ErrorManagerData _data;
    private MapData _mapData;

    public IEnumerable<ComputerErrorData> Errors => _data.Errors;

    public event Action<ComputerErrorData> ErrorAdded;
    public event Action<ComputerErrorData> ErrorChanged;
    public event Action<ComputerErrorData> ErrorDeleted;

    private void Awake() {
        _urlManager.TokenGetted += delegate { SSESetup(); };
    }

    public IEnumerator GetErrors(MapData mapData) {
        _mapData = mapData;

        string token = _urlManager.Token;
        UnityWebRequest request = RequestUtility.APIGet(_APIPathToGetErrors, token);
        yield return request.SendWebRequest();

        _data = request.ToData<ErrorManagerData>();
        GenerateErrors();
    }

    private void GenerateErrors() {
        foreach (var error in _data.Errors)
            AddError(error);
    }

    private void SSESetup() {
        Uri uri = new(RequestUtility.API_URL + _APIPathToSSE);
        HttpClient client = new();
        client.SetToken(_urlManager.Token);
        client.SetHeaders();

        EventSourceReader evt = new EventSourceReader(uri, client).Start();
        evt.MessageReceived += (object sender, EventSourceMessageEventArgs e) => {
            ErrorManagerData newData = e.ToData<ErrorManagerData>();
            CheckNewData(newData);
        };

        evt.Disconnected += (object sender, DisconnectEventArgs e) => {
            if (!Application.isPlaying)
                return;

            Debug.Log($"Переподключение: {e.ReconnectDelay} - Ошибка: {e.Exception}");
            evt.Start();
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
        string token = _urlManager.Token;
        BoolChangeData dataToSend = new(error.Id, isSolved);
        UnityWebRequest www = RequestUtility.APIPut(_APIPathToChangeError, dataToSend, token);
        yield return www.SendWebRequest();
    }

    public IEnumerator DeleteErrorInDatabase(ComputerErrorData error) {
        string token = _urlManager.Token;
        UnityWebRequest www = RequestUtility.APIDelete(_APIPathToDeleteError, error.Id, token);
        yield return www.SendWebRequest();
    }

    public IEnumerator CreateErrorInDatabase(ComputerErrorType type, string comment, int computerId) {
        int clientId = _userManager.ClientId;
        ComputerErrorData newError = new(computerId, clientId, type, comment);

        string token = _urlManager.Token;
        UnityWebRequest request = RequestUtility.APIPost(_APIPathToAddError, newError, token);
        yield return request.SendWebRequest();
    }

    private void AddError(ComputerErrorData newError) {
        ComputerData computerData = _mapData.GetComputerById(newError.ComputerId);
        newError.SetAudienceId(computerData.AudienceId);
        _data.AddError(newError);
        ErrorAdded?.Invoke(newError);
    }

    private void DeleteError(ComputerErrorData error) {
        _data.DeleteError(error);
        ErrorDeleted?.Invoke(error);
    }

    private void ChangeError(ComputerErrorData error, bool isSolved) {
        error.ChangeSolved(isSolved);
        ErrorChanged?.Invoke(error);
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
