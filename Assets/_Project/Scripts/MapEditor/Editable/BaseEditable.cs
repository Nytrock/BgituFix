using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEditable : MonoBehaviour, IResolvable {
    [SerializeField] protected EditableRenderer _renderer;
    [SerializeField] protected EditableCanvas _canvas;
    [SerializeField] protected EditableActivator _activator;
    [SerializeField, Min(0)] protected float _freePrecision;
    [SerializeField] protected EditableErrorsRenderer _errorsRenderer;

    protected MapManager _mapManager;
    protected MapEditManager _editManager;
    protected SelectManager _selectManager;
    protected BaseMapElement _parent;
    protected EditableData _baseData;

    protected bool _oldIsEditing;
    protected bool _isEditing;
    protected bool _isMoving;
    protected bool _isResizing;
    protected bool _isVerticalResizing;
    protected bool _isHorizontalResizing;

    protected Vector3 _mouseOffset;
    protected float _mouseTime = 0;
    protected readonly List<ComputerErrorData> _errors = new();

    public bool IsResizing => _isResizing;
    public bool IsMoving => _isMoving;
    public bool MouseTimeTooBig => _mouseTime > 0.15f;

    public Vector2 Size => _baseData.Size;
    public Vector2 Position => _baseData.Position;
    public Vector2 LeftBottom => _baseData.LeftBottom;
    public Vector2 RightTop => _baseData.RightTop;
    public EditableData BaseData => _baseData;

    public event Action SizeOrPositionChanged;

    protected void Update() {
        _mouseTime += Time.deltaTime;

        if (_isMoving)
            ChangePositionByMouse();

        if (_isResizing)
            ChangeSizeByMouse();
    }

    public virtual void Setup(EditableData data, BaseMapElement parent) {
        _parent = parent;
        _renderer.Setup();
        _canvas.Setup(this, _selectManager);
        _errorsRenderer.Setup();

        ChangeEditingState(false);
        SetData(data);
    }

    public virtual void SetData(EditableData data) {
        _renderer.SetData(data);
        _activator.SetSize(data.Size);
        transform.position = data.Position;
        SizeOrPositionChanged?.Invoke();
    }

    public virtual void LeftButtonDown() {
        _mouseTime = 0;
        if (!_editManager.IsEdit)
            return;

        _oldIsEditing = _isEditing;
        if (!_isEditing)
            ChangeSelectState();
        ChangeResisingAndMovingState(true);
    }

    public virtual void LeftButtonUp() {
        if (!_editManager.IsEdit)
            return;

        if (!MouseTimeTooBig && _oldIsEditing)
            ChangeSelectState();
        ChangeResisingAndMovingState(false);
    }

    public void ChangeResisingAndMovingState(bool isEdit) {
        if (_isVerticalResizing || _isHorizontalResizing || _isResizing)
            _isResizing = isEdit;
        else
            _isMoving = isEdit;

        _mouseOffset = transform.position - CameraManager.LocalMousePosition;
    }

    public void ChangeEditingState(bool isEditing) {
        _isEditing = isEditing;
        _canvas.ChangeBorderState(_isEditing);
        _renderer.ChangeEditingMode(_isEditing);
    }

    public virtual void Inject(ObjectResolver resolver) {
        _mapManager = resolver.GetInstance<MapManager>();
        _editManager = resolver.GetInstance<MapEditManager>();
        _selectManager = resolver.GetInstance<SelectManager>();
        _editManager.EditStateChanged += UpdateEditState;
        _activator.Setup(this, _selectManager);
    }

    protected virtual void UpdateEditState(bool isEdit) {
        _canvas.ChangeState(isEdit);
        _editManager.ChangeCameraMoving(true);
        _errorsRenderer.ChangeEditState(isEdit);
    }

    public abstract void CheckNewError(ComputerErrorData errorData);

    public void CheckDeletedError(ComputerErrorData errorData) {
        if (_errors.Contains(errorData)) {
            _errors.Remove(errorData);
            _errorsRenderer.RemoveError(errorData);
        }
    }

    public void CheckChangedError(ComputerErrorData errorData) {
        if (errorData.IsSolved)
            CheckDeletedError(errorData);
        else
            CheckNewError(errorData);
    }

    public void SetResizings(bool verticalResizing, bool horizontalResizing) {
        _isVerticalResizing = verticalResizing;
        _isHorizontalResizing = horizontalResizing;
    }

    public void ChangeSelectState() {
        if (Input.GetKey(KeyCode.LeftControl))
            _editManager.ChangeSelectStateOfAdditionalEditable(this);
        else
            _editManager.ChangeSelectStateOfSingleEditable(this);
    }

    protected virtual void ChangePositionByMouse() {
        Vector3 mousePosition = CameraManager.LocalMousePosition + _mouseOffset;
        mousePosition = EditorUtils.GetSnappedEditablePosition(mousePosition, _baseData.Size, GetPresicion());
        _editManager.ChangeEditablesPosition(mousePosition - transform.position);
    }

    public virtual void ChangePosition(Vector3 newPosition) {
        newPosition = newPosition.Round(3);
        if (newPosition == transform.position)
            return;

        transform.position = newPosition;
        _baseData.UpdatePosition(transform.position);
        _mouseTime += 0.2f;
        SizeOrPositionChanged?.Invoke();
    }

    protected virtual void ChangeSizeByMouse() {
        _editManager.DeselectAllNowEditablesExceptOne(this);
        Vector2 mousePosition = CameraManager.LocalMousePosition;
        float width = _baseData.Size.x, height = _baseData.Size.y;
        float centerX = transform.position.x, centerY = transform.position.y;

        if (_isHorizontalResizing)
            EditorUtils.Resize(ref width, ref centerX, _mouseOffset.x, mousePosition.x, GetPresicion());

        if (_isVerticalResizing)
            EditorUtils.Resize(ref height, ref centerY, _mouseOffset.y, mousePosition.y, GetPresicion());

        ChangePosition(new(centerX, centerY));
        ChangeSize(width, height);
    }

    public virtual void ChangeSize(float width, float height) {
        width = width.Round(3);
        height = height.Round(3);
        if (_baseData.Size == new Vector2(width, height))
            return;

        _baseData.UpdateSize(width, height);
        _renderer.SetSize(_baseData.Size);
        _mouseTime += 0.2f;
        SizeOrPositionChanged?.Invoke();
    }

    protected float GetPresicion() {
        if (Input.GetKey(KeyCode.LeftShift))
            return _freePrecision;
        return _parent.Precision;
    }
}
