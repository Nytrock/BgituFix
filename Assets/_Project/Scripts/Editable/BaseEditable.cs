using UnityEngine;

public abstract class BaseEditable : MonoBehaviour {
    [SerializeField] protected EditableRenderer _renderer;
    [SerializeField] protected EditableActivator _activator;

    protected MapManager _mapManager;
    protected SelectManager _selectManager;

    protected bool _oldIsEditing;
    protected bool _isEditing;
    protected bool _isMoving;
    protected bool _isResizing;
    protected bool _isVerticalResizing;
    protected bool _isHorizontalResizing;

    protected Vector3 _mouseOffset;
    protected float _mouseTime = 0;

    public bool MouseTimeTooBig => _mouseTime > 0.15f;
    public abstract Vector2 Size { get; }
    public abstract Vector2 Position { get; }

    protected void Update() {
        _mouseTime += Time.deltaTime;
    }

    protected void BaseSetup() {
        _renderer.Setup();
    }

    public virtual void LeftButtonDown() {
        _mouseTime = 0;
    }

    public void SetManagers(MapManager mapManager, SelectManager selectManager) {
        _mapManager = mapManager;
        _selectManager = selectManager;
        _activator.Setup(this, selectManager);
    }

    public abstract void LeftButtonUp();
    public abstract void ChangePosition(Vector3 newPosition);
    public abstract void ChangeSize(float width, float height);
}
