using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Editable<TData> : BaseEditable where TData : EditableData {
    [SerializeField] protected EditableErrorsRenderer _errorsRenderer;

    protected TData _data;
    protected readonly List<ComputerErrorData> _errors = new();

    public override Vector2 Size => _data.Size;
    public override Vector2 Position => _data.Position;
    public Vector2 LeftBottom => _data.LeftBottom;
    public Vector2 RightTop => _data.RightTop;
    public TData Data => _data;

    public event Action SizeOrPositionChanged;

    public virtual void Setup(TData data) {
        BaseSetup();
        SetData(data);
        _errorsRenderer.Setup();
    }

    public virtual void SetData(TData data) {
        _data = data;
        _renderer.SetData(_data);
        _activator.SetSize(_data.Size);
        transform.position = _data.Position;
        SizeOrPositionChanged?.Invoke();
    }

    public override void ChangePosition(Vector3 newPosition) {
        if (newPosition == transform.position)
            return;

        transform.position = newPosition;
        _data.UpdatePosition(transform.position);
        _mouseTime += 0.2f;
        SizeOrPositionChanged?.Invoke();
    }

    public override void ChangeSize(float width, float height) {
        if (_data.Size == new Vector2(width, height))
            return;

        _data.UpdateSize(width, height);
        _renderer.SetSize(_data.Size);
        _mouseTime += 0.2f;
        SizeOrPositionChanged?.Invoke();
    }

    public void CheckChangedError(ComputerErrorData errorData) {
        if (errorData.IsSolved)
            CheckDeletedError(errorData);
        else
            CheckNewError(errorData);
    }

    public abstract void CheckNewError(ComputerErrorData errorData);

    public void CheckDeletedError(ComputerErrorData errorData) {
        if (_errors.Contains(errorData)) {
            _errors.Remove(errorData);
            _errorsRenderer.RemoveError(errorData);
        }
    }
}
