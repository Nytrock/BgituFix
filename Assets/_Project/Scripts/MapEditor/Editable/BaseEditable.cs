using UnityEngine;

public abstract class BaseEditable : MonoBehaviour {
    [SerializeField] protected EditableRenderer _renderer;
    [SerializeField] protected EditableCanvas _canvas;
    [SerializeField] protected EditableActivator _activator;
    [SerializeField, Min(0)] protected float _freePrecision;

    protected MapManager _mapManager;
    protected MapEditManager _editManager;
    protected SelectManager _selectManager;
    protected BaseMapElement _parent;

    protected bool _oldIsEditing;
    protected bool _isEditing;
    protected bool _isMoving;
    protected bool _isResizing;
    protected bool _isVerticalResizing;
    protected bool _isHorizontalResizing;

    protected Vector3 _mouseOffset;
    protected float _mouseTime = 0;

    public bool IsResizing => _isResizing;
    public bool MouseTimeTooBig => _mouseTime > 0.15f;
    public abstract Vector2 Size { get; }
    public abstract Vector2 Position { get; }

    protected void Update() {
        _mouseTime += Time.deltaTime;

        if (_isMoving)
            ChangePositionByMouse();

        if (_isResizing)
            ChangeSizeByMouse();
    }

    protected void BaseSetup(BaseMapElement parent) {
        _parent = parent;
        _renderer.Setup();
        _canvas.Setup(this, _selectManager);
        ChangeEditingState(false);
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
        if (!isEdit)
            _canvas.StopResizing();
    }

    public void ChangeEditingState(bool isEditing) {
        _isEditing = isEditing;
        _canvas.ChangeBorderState(_isEditing);
        _renderer.ChangeEditingMode(_isEditing);
    }

    public void SetManagers(MapManager mapManager, MapEditManager editManager, SelectManager selectManager) {
        _mapManager = mapManager;
        _editManager = editManager;
        _selectManager = selectManager;
        _editManager.EditStateChanged += UpdateEditState;
        _activator.Setup(this, selectManager);
    }

    protected virtual void UpdateEditState(bool isEdit) {
        _canvas.ChangeState(isEdit);
        _editManager.ChangeCameraMoving(true);
    }

    public void SetResizings(bool verticalResizing, bool horizontalResizing) {
        _isVerticalResizing = verticalResizing;
        _isHorizontalResizing = horizontalResizing;
    }

    public void ChangeSelectState() {
        _editManager.ChangeSelectStateOfSingleEditable(this);
    }

    protected abstract void ChangePositionByMouse();
    public abstract void ChangePosition(Vector3 newPosition);
    protected abstract void ChangeSizeByMouse();
    public abstract void ChangeSize(float width, float height);
}
