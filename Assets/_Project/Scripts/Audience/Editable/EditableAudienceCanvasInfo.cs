using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditableAudienceCanvasInfo : EditableCanvasInfo {
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private Toggle _isComputerToggle;

    private EditableAudience _audience;

    private void Awake() {
        _nameField.onValueChanged.AddListener(UpdateAudienceName);
        _isComputerToggle.onValueChanged.AddListener(UpdateAudienceIsComputer);
    }

    private void UpdateAudienceIsComputer(bool isComputer) {
        _audience.UpdateIsComputer(isComputer);
    }

    private void UpdateAudienceName(string newName) {
        _audience.UpdateName(newName);
    }

    public override void SetEditable(BaseEditable editable) {
        base.SetEditable(editable);
        _audience = editable as EditableAudience;
    }

    protected override void UpdateState() {
        base.UpdateState();
        if (_isActive) {
            _isComputerToggle.SetIsOnWithoutNotify(_audience.IsComputer);
            _nameField.SetTextWithoutNotify(_audience.Name);
        }
    }
}
