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

    protected override void ChangeSizeByMouse() {
        Vector2 oldPosition = _data.Position;
        Vector2 oldSize = _data.Size;
        base.ChangeSizeByMouse();
        CheckNewPositionAndSize(oldPosition, oldSize);
    }

    protected override void ChangePositionByMouse() {
        Vector2 oldPosition = _data.Position;
        base.ChangePositionByMouse();
        CheckNewPositionAndSize(oldPosition, _data.Size);
    }

    private void CheckNewPositionAndSize(Vector2 oldPosition, Vector2 oldSize) {
        float audienceSizeX = _audienceData.Size.x / 2f;
        float audienceSizeY = _audienceData.Size.y / 2f;
        float computerSizeX = _data.Size.x / 2f;
        float computerSizeY = _data.Size.y / 2f;

        if (audienceSizeX.NearlyEqualOrGreater(_data.Position.x + computerSizeX) &&
            (-audienceSizeX).NearlyEqualOrLess(_data.Position.x - computerSizeX)) {
            oldPosition.x = _data.Position.x;
            oldSize.x = _data.Size.x;
        }

        if (audienceSizeY.NearlyEqualOrGreater(_data.Position.y + computerSizeY) &&
            (-audienceSizeY).NearlyEqualOrLess(_data.Position.y - computerSizeY)) {
            oldPosition.y = _data.Position.y;
            oldSize.y = _data.Size.y;
        }

        if (oldPosition != _data.Position)
            ChangePosition(oldPosition);

        if (oldSize != _data.Size)
            ChangeSize(oldSize.x, oldSize.y);
    }

    public override void Setup(ComputerData data, BaseMapElement parent) {
        base.Setup(data, parent);
        _errorsRenderer.Setup();
    }

    public override void SetData(ComputerData data) {
        base.SetData(data);
        _audienceData = _mapManager.Data.GetAudienceById(data.AudienceId);
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

    public void SetManagers(MapManager mapManager, ComputerUIManager computerUI, MapEditManager editManager, SelectManager selectManager) {
        _computerUIManager = computerUI;
        SetManagers(mapManager, editManager, selectManager);
    }

    public void UpdateNumber(string newNumber) {
        _data.UpdateNumber(newNumber);
    }
}
