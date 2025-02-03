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

    public IEnumerable<ComputerErrorData> Errors => _data.Errors;

    public event Action<ComputerErrorData> ErrorAdded;
    public event Action<ComputerErrorData> ErrorChanged;
    public event Action<ComputerErrorData> ErrorDeleted;

    public IEnumerator GetErrors(MapData mapData) {
        if (string.IsNullOrEmpty(_APIPathToGetErrors)) {
            GenerateErrors(mapData);
            yield break;
        }

        string token = _urlManager.GetParameter("token");
        UnityWebRequest www = RequestUtility.APIGet(_APIPathToGetErrors, token);
        yield return www.SendWebRequest();
        List<ComputerErrorData> errorDatas = RequestUtility.ToData<List<ComputerErrorData>>(www);

        _data = new(errorDatas);
        GenerateErrors(mapData);
    }

    private void GenerateErrors(MapData mapData) {
        foreach (var error in _data.Errors) {
            ComputerData computerData = mapData.GetComputerById(error.ComputerId);
            error.SetAudienceId(computerData.AudienceId);
            ErrorAdded?.Invoke(error);
        }
    }

    public IEnumerator ChangeErrorSolve(ComputerErrorData error, bool isSolved) {
        error.ChangeSolved(isSolved);
        ErrorChanged?.Invoke(error);

        if (string.IsNullOrEmpty(_APIPathToChangeError))
            yield break;

        string token = _urlManager.GetParameter("token");
        UnityWebRequest www = RequestUtility.APIPut(_APIPathToChangeError, error.Id, error, token);
        yield return www.SendWebRequest();
    }

    public IEnumerator DeleteError(ComputerErrorData error) {
        _data.DeleteError(error);
        ErrorDeleted?.Invoke(error);

        if (string.IsNullOrEmpty(_APIPathToDeleteError))
            yield break;

        string token = _urlManager.GetParameter("token");
        UnityWebRequest www = RequestUtility.APIDelete(_APIPathToDeleteError, error.Id, token);
        yield return www.SendWebRequest();
    }

    public IEnumerator CreateError(ComputerErrorType type, string comment, int computerId) {
        int clientId = _userManager.ClientId;
        ComputerErrorData newError = new(computerId, clientId, type, comment);
        _data.AddError(newError);
        ErrorAdded?.Invoke(newError);

        if (string.IsNullOrEmpty(_APIPathToAddError))
            yield break;

        WWWForm form = new();
        form.AddField("Id", newError.Id.ToString());
        form.AddField("ComputerId", computerId.ToString());
        form.AddField("ClientId", clientId.ToString());
        form.AddField("ErrorType", ((int)type).ToString());
        form.AddField("Comment", comment);

        string token = _urlManager.GetParameter("token");
        UnityWebRequest www = RequestUtility.APIPost(_APIPathToAddError, form, token);
        yield return www.SendWebRequest();
    }
}
