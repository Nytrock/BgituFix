using System.Collections.Generic;
using UnityEngine;

public class ProfileErrorsRenderer : MonoBehaviour {
    [SerializeField] private ProfileErrorRendererPool _pool;

    private ErrorManager _errorManager;
    private readonly List<ProfileErrorRenderer> _errors = new();
    private int _userId;

    public void Setup(UserData clientData, ErrorManager errorManager) {
        _userId = clientData.Id;
        _errorManager = errorManager;

        errorManager.ErrorAdded += CheckNewError;
        errorManager.ErrorChanged += CheckChangedError;
        errorManager.ErrorDeleted += CheckDeletedError;
    }

    private void CheckChangedError(ComputerErrorData changedError) {
        foreach (var error in _errors)
            if (error.Id == changedError.Id)
                error.SetIsSolved(changedError.IsSolved);
    }

    private void CheckNewError(ComputerErrorData error) {
        if (_userId != error.UserId)
            return;

        ProfileErrorRenderer errorRenderer = _pool.GetObject();
        _errors.Add(errorRenderer);
        errorRenderer.SetError(error, _errorManager);
    }

    private void CheckDeletedError(ComputerErrorData deletedError) {
        foreach (var error in _errors) {
            if (error.Id == deletedError.Id) {
                _errors.Remove(error);
                _pool.PutObject(error);
            }
        }
    }
}
