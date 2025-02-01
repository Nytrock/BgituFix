using TMPro;
using UnityEngine;

public class EditableAudience : Editable<AudienceData> {
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private ComputerErrorRenderer _errorRenderer;

    public override void Press() {
        _mapManager.OpenAudience(_data);
    }

    public override void Setup(AudienceData data) {
        base.Setup(data);
        _nameText.text = data.Name;

        ComputerErrorType type = _mapManager.Data.GetAudienceMaxErrorType(_data.Id);
        _errorRenderer.ChangeState(type != ComputerErrorType.None);
        _errorRenderer.SetVisual(type);
    }
}
