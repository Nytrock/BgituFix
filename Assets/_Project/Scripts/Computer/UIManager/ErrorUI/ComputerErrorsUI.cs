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

    private void Awake() {
        _typeFilter.onValueChanged.AddListener(delegate { UpdateFilters(); });
        _solvedFilter.onValueChanged.AddListener(delegate { UpdateFilters(); });
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
        ClearList();

        foreach (var error in _errorManager.Errors)
            if (error.ComputerId == data.Id)
                CreateErrorUI(error, _userManager);
    }

    private void CreateErrorUI(ComputerErrorData error, UserManager userManager) {
        ComputerErrorUI errorUI = _errorsPool.GetObject();
        errorUI.SetError(error, userManager);
        _errors.Add(errorUI);
    }

    private void ClearList() {
        foreach (var error in _errors)
            _errorsPool.PutObject(error);
        _errors.Clear();
    }

    public void ChangeErrorSolve(ComputerErrorData error, bool isSolved) {
        StartCoroutine(_errorManager.ChangeErrorSolve(error, isSolved));
        UpdateFilters();
    }

    public void DeleteError(ComputerErrorData error, ComputerErrorUI errorUI) {
        _errorsPool.PutObject(errorUI);
        StartCoroutine(_errorManager.DeleteError(error));
    }
}
