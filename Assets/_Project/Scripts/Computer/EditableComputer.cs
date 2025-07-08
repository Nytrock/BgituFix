using UnityEngine;

public class EditableComputer : Editable<ComputerData> {
    private EditableComputerRenderer _computerRenderer;

    private ComputerUIManager _computerUIManager;
    private AudienceData _audienceData;

    protected void Awake() {
        _computerRenderer = _renderer as EditableComputerRenderer;
    }

    public override void LeftButtonUp() {
        base.LeftButtonUp();
        if (_editManager.IsEdit || MouseTimeTooBig)
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

    public override void SetData(ComputerData data) {
        base.SetData(data);
        _audienceData = _mapManager.Data.GetAudienceById(data.AudienceId);
    }

    public void SetManagers(MapManager mapManager, ComputerUIManager computerUI, MapEditManager editManager, SelectManager selectManager) {
        _computerUIManager = computerUI;
        SetManagers(mapManager, editManager, selectManager);
    }

    public void UpdateNumber(string newNumber) {
        _data.UpdateNumber(newNumber);
    }

    public override void CheckNewError(ComputerErrorData errorData) {
        if (_data.Id == errorData.ComputerId && !errorData.IsSolved) {
            _errors.Add(errorData);
            _errorsRenderer.AddError(errorData);
        }
    }

    public void UpdateType(ComputerType computerType) {
        _data.UpdateType(computerType);
        _computerRenderer.UpdateType(computerType);
    }
}
