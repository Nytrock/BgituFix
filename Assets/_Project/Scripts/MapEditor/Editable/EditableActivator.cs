using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class EditableActivator : MonoBehaviour {
    protected SpriteRenderer _renderer;
    private BoxCollider2D _collider;
    private bool _isMouseHold;

    public event Action Pressed;

    private void GetComponents() {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }

    protected virtual void OnMouseDown() {
        _isMouseHold = true;
    }

    private void OnMouseUp() {
        if (!_isMouseHold)
            return;

        Pressed?.Invoke();
    }

    private void OnMouseExit() {
        _isMouseHold = false;
    }

    public virtual void Setup(EditableData data) {
        GetComponents();
        _renderer.size = data.Size;
        _collider.size = data.Size;
    }
}
