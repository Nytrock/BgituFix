using UnityEngine;

public abstract class BaseEditable : MonoBehaviour {
    [SerializeField] protected EditableRenderer _renderer;

    protected MapManager _mapManager;

    protected bool _oldIsEditing;
    protected bool _isEditing;
    protected bool _isMoving;
    protected bool _isResizing;
    protected bool _isVerticalResizing;
    protected bool _isHorizontalResizing;

    protected Vector3 _mouseOffset;
    protected float _mouseTime = 0;

    public bool IsResizing => _isResizing;
    public abstract Vector2 Size { get; }
    public abstract Vector2 Position { get; }

    private void Update() {
        _mouseTime += Time.deltaTime;

        if (_isMoving)
            UpdatePosition();

        if (_isResizing)
            UpdateSize();
    }

    public abstract void Click();

    public void SetManagers(MapManager mapManager) {
        _mapManager = mapManager;
    }

    protected abstract void UpdatePosition();
    protected abstract void UpdateSize();
}
