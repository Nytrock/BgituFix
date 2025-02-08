using System.Collections.Generic;
using UnityEngine;

public class EditableComputer : Editable<ComputerData> {
    [SerializeField] private EditableComputerErrorsRenderer _errorsRenderer;

    private ComputerUIManager _computerUIManager;
    private AudienceData _audienceData;
    private readonly List<ComputerErrorData> _errors = new();

    public string SerialNumber => _data.SerialNumber;

    public override void LeftButtonUp() {
        _computerUIManager.OpenComputer(_data);
    }

    protected override void UpdateSize() {
        Vector2 oldPosition = _data.PositionVector;
        Vector2 oldSize = _data.SizeVector;
        base.UpdateSize();
        CheckNewPositionAndSize(oldPosition, oldSize);
    }

    protected override void UpdatePosition() {
        Vector2 oldPosition = _data.PositionVector;
        base.UpdatePosition();
        CheckNewPositionAndSize(oldPosition, _data.SizeVector);
    }

    private void CheckNewPositionAndSize(Vector2 oldPosition, Vector2 oldSize) {
        float audienceSizeX = _audienceData.SizeVector.x / 2f;
        float audienceSizeY = _audienceData.SizeVector.y / 2f;
        float computerSizeX = _data.SizeVector.x / 2f;
        float computerSizeY = _data.SizeVector.y / 2f;

        if (audienceSizeX > _data.PositionVector.x + computerSizeX &&
            -audienceSizeX < _data.PositionVector.x - computerSizeX) {
            oldPosition.x = _data.PositionVector.x;
            oldSize.x = _data.SizeVector.x;
        }

        if (audienceSizeY > _data.PositionVector.y + computerSizeY &&
            -audienceSizeY < _data.PositionVector.y - computerSizeY) {
            oldPosition.y = _data.PositionVector.y;
            oldSize.y = _data.SizeVector.y;
        }

        if (oldPosition != _data.PositionVector) {
            transform.position = oldPosition;
            _data.UpdatePosition(oldPosition);
        }

        if (oldSize != _data.SizeVector) {
            _data.UpdateSize(oldSize.x, oldSize.y);
            _renderer.SetSize(oldSize);
        }
    }

    public override void Setup(ComputerData data) {
        base.Setup(data);
        _errorsRenderer.Setup();
        _audienceData = _mapManager.Data.GetAudienceById(data.AudienceId);
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

    public void SetManagers(MapManager mapManager, ComputerUIManager computerUI) {
        _computerUIManager = computerUI;
        SetManagers(mapManager);
    }

    public void UpdateNumber(string newNumber) {
        _data.UpdateNumber(newNumber);
    }
}
