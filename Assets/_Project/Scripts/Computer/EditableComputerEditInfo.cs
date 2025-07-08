using System.Linq;
using TMPro;
using UnityEngine;

public class EditableComputerEditInfo : EditableCanvasInfo {
    [SerializeField] private TMP_InputField _numberField;
    [SerializeField] private CustomDropdown _typeDropdown;

    private EditableComputer _computer;

    private void Awake() {
        _editManager.EditablesChanged += CheckEditables;
        _editManager.EditStateChanged += delegate { ChangeState(false); };
        ChangeState(false);

        _numberField.onValueChanged.AddListener(UpdateComputerNumber);
        _typeDropdown.onValueChanged.AddListener(UpdateComputerType);
    }

    private void CheckEditables() {
        if (_editManager.NowEditables.Count() != 1) {
            ResetComputer();
            return;
        }

        EditableComputer computer = _editManager.GetEditable(0) as EditableComputer;
        if (computer == null)
            return;

        SetComputer(computer);
    }

    private void UpdateComputerNumber(string newNumber) {
        if (string.IsNullOrEmpty(newNumber))
            return;

        _computer.UpdateNumber(newNumber);
    }

    private void UpdateComputerType(int type) {
        ComputerType computerType = (ComputerType)type;
        _computer.UpdateType(computerType);
    }

    private void SetComputer(EditableComputer computer) {
        _computer = computer;
        _typeDropdown.SetValueWithoutNotify((int)_computer.Data.Type);
        _numberField.SetTextWithoutNotify(_computer.Data.SerialNumber);
        ChangeState(true);
    }

    private void ResetComputer() {
        _computer = null;
        ChangeState(false);
    }
}
