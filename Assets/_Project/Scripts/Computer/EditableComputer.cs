using System.Collections.Generic;
using UnityEngine;

public class EditableComputer : Editable<ComputerData> {
    [SerializeField] private EditableComputerErrorsRenderer _errorsRenderer;

    private ComputerUIManager _computerUIManager;
    private readonly List<ComputerErrorData> _errors = new();

    public override void Setup(ComputerData data) {
        base.Setup(data);
        _errorsRenderer.Setup();
    }

    public void CheckChangedError(ComputerErrorData errorData) {
        if (errorData.IsSolved)
            CheckDeletedError(errorData);
        else
            CheckNewError(errorData);
    }

    public void CheckNewError(ComputerErrorData errorData) {
        if (_data.Id == errorData.ComputerId && !errorData.IsSolved) {
            _errors.Add(errorData);
            _errorsRenderer.AddError(errorData);
        }
    }

    public void CheckDeletedError(ComputerErrorData errorData) {
        if (_errors.Contains(errorData)) {
            _errors.Remove(errorData);
            _errorsRenderer.RemoveError(errorData);
        }
    }

    public override void Press() {
        _computerUIManager.OpenComputer(_data);
    }

    public void SetUI(ComputerUIManager computerUIManager) {
        _computerUIManager = computerUIManager;
    }
}
