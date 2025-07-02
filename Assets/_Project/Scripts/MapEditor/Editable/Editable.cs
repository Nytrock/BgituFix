using System;
using UnityEngine;

public abstract class Editable<TData> : BaseEditable
    where TData : EditableData {

    [SerializeField, Min(0)] protected float _gridPrecision;

    protected TData _data;

    public override Vector2 Size => _data.Size;
    public override Vector2 Position => _data.Position;
    public Vector2 LeftBottom => _data.LeftBottom;
    public Vector2 RightTop => _data.RightTop;
    public TData Data => _data;

    public event Action SizeOrPositionChanged;

    public virtual void Setup(TData data) {
        SetData(data);
    }

    public virtual void SetData(TData data) {
        _data = data;
        _renderer.SetData(_data);
        transform.position = _data.Position;
    }

    protected override void UpdatePosition() {
        ChangePosition(GetSnappedPosition(CameraManager.LocalMousePosition + _mouseOffset, _gridPrecision));
        SizeOrPositionChanged?.Invoke();
    }

    protected override void UpdateSize() {
        Vector2 mousePosition = CameraManager.LocalMousePosition;
        float width = _data.Size.x, height = _data.Size.y;
        float centerX = transform.position.x, centerY = transform.position.y;

        if (_isHorizontalResizing)
            Resize(ref width, ref centerX, _mouseOffset.x, mousePosition.x);

        if (_isVerticalResizing)
            Resize(ref height, ref centerY, _mouseOffset.y, mousePosition.y);

        ChangePosition(new(centerX, centerY));
        ChangeSize(width, height);
        SizeOrPositionChanged?.Invoke();
    }

    private void Resize(ref float length, ref float center, float mouseOffset, float mousePosition) {
        int sign = mouseOffset > 0 ? 1 : -1;
        float border = center + length / 2f * sign;
        if (border * sign < mousePosition * sign)
            return;

        float rawWidth = Mathf.Abs(border - mousePosition);
        length = SnapToGrid(rawWidth, _gridPrecision * 2f);
        length = Mathf.Max(length, _gridPrecision * 2);
        center = border - length / 2f * sign;
    }

    protected Vector3 GetSnappedPosition(Vector3 rawPosition, float precision) {
        float x = SnapToGrid(rawPosition.x, precision);
        float y = SnapToGrid(rawPosition.y, precision);
        return new(x, y);
    }

    private float SnapToGrid(float value, float precision) {
        return Mathf.Round(value / precision) * precision;
    }

    public void ChangePosition(Vector2 newPosition) {
        transform.position = newPosition;
        _data.UpdatePosition(transform.position);
    }

    public void ChangeSize(float width, float height) {
        _data.UpdateSize(width, height);
        _renderer.SetSize(_data.Size);
    }
}
