public class EditableAudience : Editable<AudienceData> {
    private EditableAudienceRenderer _audienceActivator;

    public ComputerErrorType ErrorType => _errorsRenderer.GetMaxError();
    public int Floor => _data.Floor;
    public bool IsComputer => _data.IsComputer;
    public string Name => _data.Name;

    protected void Awake() {
        _audienceActivator = _renderer as EditableAudienceRenderer;
    }

    public override void LeftButtonUp() {
        base.LeftButtonUp();
        if (_editManager.IsEdit || !_data.IsComputer || MouseTimeTooBig)
            return;

        _mapManager.OpenAudience(_data);
    }

    public override void CheckNewError(ComputerErrorData data) {
        if (data.AudienceId == _data.Id && !data.IsSolved) {
            _errors.Add(data);
            _errorsRenderer.AddError(data);
        }
    }

    public void UpdateIsComputer(bool isComputer) {
        _data.UpdateIsComputer(isComputer);
        _audienceActivator.UpdateStyle();
    }

    public void UpdateName(string newName) {
        _data.UpdateName(newName);
        _audienceActivator.UpdateName();
    }
}
