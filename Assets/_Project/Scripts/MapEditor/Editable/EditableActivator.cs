using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EditableActivator : MonoBehaviour {
    private BaseEditable _editable;
    private SelectManager _selectManager;
    private BoxCollider2D _collider;

    public BaseEditable Editable => _editable;

    private void OnMouseEnter() {
        _selectManager.SetEditable(_editable);
    }

    private void OnMouseExit() {
        _selectManager.ResetEditable();
    }

    public void Update() {
        if (Input.GetMouseButtonUp(0) && _editable.IsResizing)
            _editable.ChangeResisingAndMovingState(false);
    }

    public void Setup(BaseEditable editable, SelectManager selectManager) {
        _editable = editable;
        _selectManager = selectManager;
        _collider = GetComponent<BoxCollider2D>();
    }

    public void SetSize(Vector2 size) {
        _collider.size = size;
    }
}
