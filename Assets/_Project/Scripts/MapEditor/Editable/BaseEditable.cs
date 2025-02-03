using UnityEngine;

public abstract class BaseEditable : MonoBehaviour {
    [SerializeField] protected EditableActivator _activator;
    [SerializeField] protected EditableCanvas _canvas;

    protected MapManager _mapManager;
    protected MapEditManager _editManager;

    protected bool _oldIsEditing;
    protected bool _isEditing;
    protected bool _isMoving;
    protected bool _isResizing;
    protected bool _isVerticalResizing;
    protected bool _isHorizontalResizing;

    protected Vector3 _mouseOffset;
    protected float _mouseTime = 0;

    public bool IsResizing => _isResizing;

    private void Awake() {
        _activator.LeftButtonDown += LeftButtonDown;
        _activator.LeftButtonUp += LeftButtonUp;
        _activator.RightButtonUp += RightButtonUp;
    }

    private void Update() {
        _mouseTime += Time.deltaTime;

        if (_isMoving)
            UpdatePosition();

        if (_isResizing)
            UpdateSize();
    }

    public virtual void LeftButtonDown() {
        if (!_editManager.IsEdit)
            return;

        _oldIsEditing = _isEditing;
        if (!_isEditing)
            _editManager.SetEditable(this);
        _mouseTime = 0;
        ChangeEditState(true);
    }

    protected virtual void LeftButtonUp() {
        if (!_editManager.IsEdit)
            return;

        if (_mouseTime < 0.1f && _oldIsEditing)
            _editManager.SetEditable(this);
        ChangeEditState(false);
    }

    public void RightButtonUp() {
        if (!_editManager.IsEdit)
            return;

        _canvas.ChangeInfoState();
        if (!_isEditing)
            _editManager.SetEditable(this);
    }

    private void ChangeEditState(bool isEdit) {
        if (_isVerticalResizing || _isHorizontalResizing || _isResizing)
            _isResizing = isEdit;
        else
            _isMoving = isEdit;

        _mouseOffset = transform.position - CameraManager.LocalMousePosition;
        if (!isEdit)
            _canvas.ChangeResizeSettings(CursorType.Default);
    }

    public void ChangeEditingMode(bool isEditing) {
        _isEditing = isEditing;
        _activator.ChangeEditingMode(isEditing);
        _canvas.ChangeBorderState(_isEditing);
        if (!_isEditing)
            _canvas.ChangeInfoState(false);
    }

    public void SetManagers(MapManager mapManager, MapEditManager editManager) {
        _mapManager = mapManager;
        _editManager = editManager;
        _editManager.EditStateChanged += UpdateEditState;
    }

    protected virtual void UpdateEditState(bool isEdit) {
        _canvas.ChangeState(isEdit);
    }

    public void SetResizings(bool verticalResizing, bool horizontalResizing) {
        _isVerticalResizing = verticalResizing;
        _isHorizontalResizing = horizontalResizing;
    }

    protected abstract void UpdatePosition();
    protected abstract void UpdateSize();
}
