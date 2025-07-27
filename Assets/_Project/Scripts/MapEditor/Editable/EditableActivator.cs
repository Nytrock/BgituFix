using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EditableActivator : MonoBehaviour, ISelectable {
    private BaseEditable _editable;
    private BoxCollider2D _collider;

    public BaseEditable Editable => _editable;

    public void Setup(BaseEditable editable, SelectManager selectManager) {
        _editable = editable;
        _collider = GetComponent<BoxCollider2D>();
    }

    public void SetSize(Vector2 size) {
        _collider.size = size;
    }

    public void MouseDown() {
        _editable.LeftButtonDown();
    }

    public void MouseUp() {
        _editable.LeftButtonUp();
    }
}
