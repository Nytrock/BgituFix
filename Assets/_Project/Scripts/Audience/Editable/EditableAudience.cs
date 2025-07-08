public class EditableAudience : Editable<AudienceData> {
    private EditableAudienceRenderer _audienceRenderer;

    public ComputerErrorType ErrorType => _errorsRenderer.GetMaxError();
    public int Floor => _data.Floor;

    protected void Awake() {
        _audienceRenderer = _renderer as EditableAudienceRenderer;
    }

    public override void LeftButtonUp() {
        base.LeftButtonUp();
        if (_editManager.IsEdit || _data.Type != AudienceType.Computer || MouseTimeTooBig)
            return;

        _mapManager.OpenAudience(_data);
    }

    public override void CheckNewError(ComputerErrorData data) {
        if (data.AudienceId == _data.Id && !data.IsSolved) {
            _errors.Add(data);
            _errorsRenderer.AddError(data);
        }
    }

    public void UpdateType(AudienceType type) {
        _data.UpdateType(type);
        _audienceRenderer.UpdateStyle();
    }

    public void UpdateName(string newName) {
        _data.UpdateName(newName);
        _audienceRenderer.UpdateName();
    }
}
