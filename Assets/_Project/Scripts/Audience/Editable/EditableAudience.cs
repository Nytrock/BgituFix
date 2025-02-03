using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EditableAudience : Editable<AudienceData> {
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private ComputerErrorRenderer _errorRenderer;

    private readonly List<ComputerErrorData> _errors = new();

    public ComputerErrorType ErrorType => _errorRenderer.Type;
    public int Floor => _data.Floor;

    protected override void LeftButtonUp() {
        base.LeftButtonUp();
        if (_editManager.IsEdit || !_data.IsComputer)
            return;

        _mapManager.OpenAudience(_data);
    }

    protected override void UpdateEditState(bool isEdit) {
        base.UpdateEditState(isEdit);
        UpdateRenderer();
    }

    public override void Setup(AudienceData data) {
        base.Setup(data);
        _nameText.text = data.Name;
        _errorRenderer.ChangeState(false);
    }

    public void CheckChangedError(ComputerErrorData data) {
        if (data.IsSolved)
            CheckDeletedError(data);
        else
            CheckNewError(data);
    }

    public void CheckNewError(ComputerErrorData data) {
        if (data.AudienceId == _data.Id) {
            _errors.Add(data);
            UpdateRenderer();
        }
    }

    public void CheckDeletedError(ComputerErrorData data) {
        if (_errors.Contains(data)) {
            _errors.Remove(data);
            UpdateRenderer();
        }
    }

    private void UpdateRenderer() {
        _errorRenderer.ChangeState(_errors.Count != 0 && !_editManager.IsEdit);
        if (_errors.Count > 0)
            _errorRenderer.SetType(_errors.Select(error => error.Type).Max());
    }
}
