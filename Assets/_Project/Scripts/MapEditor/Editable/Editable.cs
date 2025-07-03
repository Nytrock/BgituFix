using System;
using UnityEngine;

public abstract class Editable<TData> : BaseEditable
    where TData : EditableData {

    protected TData _data;

    public override Vector2 Size => _data.Size;
    public override Vector2 Position => _data.Position;
    public Vector2 LeftBottom => _data.LeftBottom;
    public Vector2 RightTop => _data.RightTop;
    public TData Data => _data;

    public event Action SizeOrPositionChanged;

    public virtual void Setup(TData data, BaseMapElement parent) {
        BaseSetup(parent);
        SetData(data);
    }

    public virtual void SetData(TData data) {
        _data = data;
        _renderer.SetData(_data);
        _activator.SetSize(_data.Size);
        transform.position = _data.Position;
        SizeOrPositionChanged?.Invoke();
    }

    protected override void ChangePositionByMouse() {
        Vector3 mousePosition = CameraManager.LocalMousePosition + _mouseOffset;
        mousePosition = CalculationUtils.GetSnappedPosition(mousePosition, _gridPrecision);
        _editManager.ChangeEditablesPosition(mousePosition - transform.position);
    }

    protected override void ChangeSizeByMouse() {
        _editManager.DeselectAllNowEditablesExceptOne(this);
        Vector2 mousePosition = CameraManager.LocalMousePosition;
        float width = _data.Size.x, height = _data.Size.y;
        float centerX = transform.position.x, centerY = transform.position.y;

        if (_isHorizontalResizing)
            CalculationUtils.Resize(ref width, ref centerX, _mouseOffset.x, mousePosition.x, _gridPrecision);

        if (_isVerticalResizing)
            CalculationUtils.Resize(ref height, ref centerY, _mouseOffset.y, mousePosition.y, _gridPrecision);

        ChangePosition(new(centerX, centerY));
        ChangeSize(width, height);
    }

    public override void ChangePosition(Vector2 newPosition) {
        transform.position = newPosition;
        _data.UpdatePosition(transform.position);
        SizeOrPositionChanged?.Invoke();
    }

    public override void ChangeSize(float width, float height) {
        _data.UpdateSize(width, height);
        _renderer.SetSize(_data.Size);
        SizeOrPositionChanged?.Invoke();
    }
}
