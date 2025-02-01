using UnityEngine;

public class EditableComputer : Editable<ComputerData> {
    [SerializeField] private EditableComputerErrorsRenderer _errors;
    private ComputerUIManager _computerUIManager;

    public override void Setup(ComputerData data) {
        base.Setup(data);
        _errors.Setup();
        foreach (var error in _mapManager.Data.GetErrorsByComputer(data))
            if (!error.IsSolved)
                _errors.AddError(error);
    }

    public override void Press() {
        _computerUIManager.OpenComputer(_data);
    }

    public void SetUI(ComputerUIManager computerUIManager) {
        _computerUIManager = computerUIManager;
    }
}
