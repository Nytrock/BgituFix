using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class EditableActivator : MonoBehaviour {
    private SpriteRenderer _renderer;
    private BoxCollider2D _collider;

    public event Action Pressed;

    private void GetComponents() {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown() {
        Pressed?.Invoke();
    }

    public virtual void Setup(EditableData data) {
        GetComponents();
        _renderer.size = data.Size;
        _collider.size = data.Size;
    }
}
