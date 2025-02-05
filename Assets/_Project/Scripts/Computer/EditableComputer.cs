using System.Collections.Generic;
using UnityEngine;

public class EditableComputer : Editable<ComputerData> {
    [SerializeField] private EditableComputerErrorsRenderer _errorsRenderer;

    private ComputerUIManager _computerUIManager;
    private AudienceData _audienceData;
    private readonly List<ComputerErrorData> _errors = new();

    public string SerialNumber => _data.SerialNumber;

    public override void LeftButtonUp() {
        base.LeftButtonUp();
        if (_editManager.IsEdit)
            return;

        _computerUIManager.OpenComputer(_data);
    }

    protected override void UpdateSize() {
        Vector2 oldPosition = _data.Position;
        Vector2 oldSize = _data.Size;
        base.UpdateSize();
        CheckNewPositionAndSize(oldPosition, oldSize);
    }

    protected override void UpdatePosition() {
        Vector2 oldPosition = _data.Position;
        base.UpdatePosition();
        CheckNewPositionAndSize(oldPosition, _data.Size);
    }

    private void CheckNewPositionAndSize(Vector2 oldPosition, Vector2 oldSize) {
        float audienceSizeX = _audienceData.Size.x / 2f;
        float audienceSizeY = _audienceData.Size.y / 2f;
        float computerSizeX = _data.Size.x / 2f;
        float computerSizeY = _data.Size.y / 2f;

        if (audienceSizeX > _data.Position.x + computerSizeX &&
            -audienceSizeX < _data.Position.x - computerSizeX) {
            oldPosition.x = _data.Position.x;
            oldSize.x = _data.Size.x;
        }

        if (audienceSizeY > _data.Position.y + computerSizeY &&
            -audienceSizeY < _data.Position.y - computerSizeY) {
            oldPosition.y = _data.Position.y;
            oldSize.y = _data.Size.y;
        }

        if (oldPosition != _data.Position) {
            transform.position = oldPosition;
            _data.UpdatePosition(oldPosition);
        }

        if (oldSize != _data.Size) {
            _data.UpdateSize(oldSize.x, oldSize.y);
            _renderer.SetSize(oldSize);
        }
    }

    public override void Setup(ComputerData data) {
        base.Setup(data);
        _errorsRenderer.Setup();
        _audienceData = _mapManager.Data.GetAudienceById(data.Id);
    }

    protected override void UpdateEditState(bool isEdit) {
        base.UpdateEditState(isEdit);
        _errorsRenderer.ChangeState(!isEdit);
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

    public void SetManagers(MapManager mapManager, ComputerUIManager computerUI, MapEditManager editManager) {
        _computerUIManager = computerUI;
        SetManagers(mapManager, editManager);
    }

    public void UpdateNumber(string newNumber) {
        _data.UpdateNumber(newNumber);
    }

    public override void Copy() {
        ComputerData computerData = new(_data, _gridPrecision * 2);
        _editManager.CreateComputer(computerData);
    }

    public override void Delete() {
        _editManager.DeleteComputer(this);
    }
}
