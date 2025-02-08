using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComputerErrorsUI : MonoBehaviour {
    [SerializeField] private ErrorManager _errorManager;
    [SerializeField] private UserManager _userManager;
    [SerializeField] private ComputerErrorUIPool _errorsPool;
    [SerializeField] private TMP_Dropdown _typeFilter;
    [SerializeField] private TMP_Dropdown _solvedFilter;

    private readonly List<ComputerErrorUI> _errors = new();
    private ComputerData _computerData;

    private void Awake() {
        _typeFilter.onValueChanged.AddListener(delegate { UpdateFilters(); });
        _solvedFilter.onValueChanged.AddListener(delegate { UpdateFilters(); });

        _errorManager.ErrorAdded += CheckNewError;
        _errorManager.ErrorChanged += CheckChangedError;
        _errorManager.ErrorDeleted += CheckDeletedError;
    }

    private void UpdateFilters() {
        ComputerErrorType type = (ComputerErrorType)_typeFilter.value;
        bool isSolved = _solvedFilter.value == 2;

        foreach (var error in _errors) {
            bool isActive = true;
            if (type != ComputerErrorType.None)
                isActive &= type == error.ErrorType;
            if (_solvedFilter.value != 0)
                isActive &= error.IsSolved == isSolved;
            error.ChangeState(isActive);
        }
    }

    public void SetupErrorsList(ComputerData data) {
        _computerData = data;
        _solvedFilter.value = 0;
        _typeFilter.value = 0;

        foreach (var error in _errorManager.Errors)
            if (error.ComputerId == data.Id)
                CreateErrorUI(error);
    }

    private void CreateErrorUI(ComputerErrorData error) {
        ComputerErrorUI errorUI = _errorsPool.GetObject();
        errorUI.SetError(error, _userManager);
        _errors.Add(errorUI);
    }

    public void ChangeErrorSolve(ComputerErrorData error, bool isSolved) {
        StartCoroutine(_errorManager.ChangeErrorSolveInDatabase(error, isSolved));
    }

    public void DeleteError(ComputerErrorData error) {
        StartCoroutine(_errorManager.DeleteErrorInDatabase(error));
    }

    public void Close() {
        _computerData = null;
        foreach (var error in _errors)
            _errorsPool.PutObject(error);
        _errors.Clear();
    }

    private void CheckChangedError(ComputerErrorData data) {
        foreach (var error in _errors)
            if (error.Id == data.Id)
                error.UpdateSolved();
        UpdateFilters();
    }

    private void CheckNewError(ComputerErrorData data) {
        if (data.ComputerId == _computerData.Id) {
            CreateErrorUI(data);
            UpdateFilters();
        }
    }

    private void CheckDeletedError(ComputerErrorData data) {
        List<ComputerErrorUI> errorsToDelete = new();
        foreach (var error in _errors)
            if (error.Id == data.Id)
                errorsToDelete.Add(error);

        foreach (var error in errorsToDelete) {
            _errors.Remove(error);
            _errorsPool.PutObject(error);
        }
    }
}
