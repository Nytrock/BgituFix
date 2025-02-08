using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private ErrorManagerData _data;
    private MapData _mapData;

    public IEnumerable<ComputerErrorData> Errors => _data.Errors;

    public event Action<ComputerErrorData> ErrorAdded;
    public event Action<ComputerErrorData> ErrorChanged;
    public event Action<ComputerErrorData> ErrorDeleted;

    private void Awake() {
        _urlManager.TokenGetted += delegate { StartCoroutine(SSESurrogate()); };
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

    private IEnumerator SSESurrogate() {
        if (_urlManager.Token == string.Empty)
            yield break;

        WaitForSeconds wait = new(120);
        yield return wait;

        while (true) {
            string token = _urlManager.Token;
            UnityWebRequest request = RequestUtility.APIGet(_APIPathToGetErrors, token);
            yield return request.SendWebRequest();

            ErrorManagerData newData = request.ToData<ErrorManagerData>();
            CheckNewData(newData);
            yield return wait;
        }
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
        UnityWebRequest request = RequestUtility.APIPut(_APIPathToChangeError, dataToSend, token);
        yield return request.SendWebRequest();

        ChangeError(error, isSolved);
    }

    public IEnumerator DeleteErrorInDatabase(ComputerErrorData error) {
        string token = _urlManager.Token;
        UnityWebRequest request = RequestUtility.APIDelete(_APIPathToDeleteError, error.Id, token);
        yield return request.SendWebRequest();

        DeleteError(error);
    }

    public IEnumerator CreateErrorInDatabase(ComputerErrorType type, string comment, int computerId) {
        int clientId = _userManager.ClientId;
        ComputerErrorData newError = new(computerId, clientId, type, comment);

        string token = _urlManager.Token;
        UnityWebRequest request = RequestUtility.APIPost(_APIPathToAddError, newError, token);
        yield return request.SendWebRequest();
        IdData idData = request.ToData<IdData>();

        newError.SetId(idData.Id);
        AddError(newError);
    }

    private void AddError(ComputerErrorData newError) {
        ComputerData computerData = _mapData.GetComputerById(newError.ComputerId);
        newError.SetAudienceId(computerData.AudienceId);
        Debug.Log(computerData.AudienceId);
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
