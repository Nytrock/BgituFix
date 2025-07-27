using System.Linq;
using TMPro;
using UnityEngine;

public class EditableAudienceEditInfo : EditableCanvasInfo {
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private CustomDropdown _typeDropdown;

    private EditableAudience _audience;

    private void Awake() {
        _editManager.EditablesChanged += CheckEditables;
        _editManager.EditStateChanged += delegate { ChangeState(false); };
        ChangeState(false);

        _nameField.onValueChanged.AddListener(UpdateAudienceName);
        _typeDropdown.onValueChanged.AddListener(UpdateAudienceType);
    }

    private void CheckEditables() {
        if (_editManager.NowEditables.Count() != 1) {
            ResetAudience();
            return;
        }

        EditableAudience audience = _editManager.GetEditable(0) as EditableAudience;
        if (audience == null)
            return;

        SetAudience(audience);
    }

    private void UpdateAudienceType(int type) {
        AudienceType audienceType = (AudienceType)(type + 1);
        _audience.UpdateType(audienceType);
    }

    private void UpdateAudienceName(string newName) {
        _audience.UpdateName(newName);
    }

    private void SetAudience(EditableAudience audience) {
        _audience = audience;
        _typeDropdown.SetValueWithoutNotify((int)(_audience.Data.Type - 1));
        _nameField.SetTextWithoutNotify(_audience.Data.Name);
        ChangeState(true);
    }

    private void ResetAudience() {
        _audience = null;
        ChangeState(false);
    }
}
