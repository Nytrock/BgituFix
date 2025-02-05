using TMPro;
using UnityEngine;

public class EditableComputerCanvasInfo : EditableCanvasInfo {
    [SerializeField] private TMP_InputField _numberField;

    private EditableComputer _computer;

    private void Awake() {
        _numberField.onValueChanged.AddListener(UpdateComputerNumber);
    }

    public override void SetEditable(BaseEditable editable) {
        base.SetEditable(editable);
        _computer = editable as EditableComputer;
    }

    protected override void UpdateState() {
        base.UpdateState();
        if (_isActive)
            _numberField.SetTextWithoutNotify(_computer.SerialNumber);
    }

    private void UpdateComputerNumber(string newNumber) {
        if (string.IsNullOrEmpty(newNumber))
            return;

        _computer.UpdateNumber(newNumber);
    }
}
