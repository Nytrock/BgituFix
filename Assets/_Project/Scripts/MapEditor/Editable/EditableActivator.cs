using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class EditableActivator : MonoBehaviour {
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private float _canvasMultiplier;

    protected SpriteRenderer _renderer;
    private BoxCollider2D _collider;
    private bool _isHover;
    private bool _isEditing;

    public event Action LeftButtonDown;
    public event Action LeftButtonUp;
    public event Action RightButtonDown;
    public event Action RightButtonUp;

    private void GetComponents() {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update() {
        if (!_isHover && _isEditing) {
            CheckButtonsUp();
            return;
        }

        if (!_isHover)
            return;

        CheckButtonsUp();
        CheckButtonsDown();
    }

    private void CheckButtonsDown() {
        if (Input.GetMouseButtonDown(0))
            LeftButtonDown?.Invoke();
        if (Input.GetMouseButtonDown(1))
            RightButtonDown?.Invoke();
    }

    private void CheckButtonsUp() {
        if (Input.GetMouseButtonUp(0))
            LeftButtonUp?.Invoke();
        if (Input.GetMouseButtonUp(1))
            RightButtonUp?.Invoke();
    }

    private void OnMouseEnter() {
        _isHover = true;
    }

    private void OnMouseExit() {
        _isHover = false;
    }

    private void OnDisable() {
        _isHover = false;
    }

    public virtual void Setup(EditableData data) {
        GetComponents();
        SetSize(data.Size);
    }

    public void SetSize(Vector2 size) {
        _renderer.size = size;
        _collider.size = size;
        _canvas.sizeDelta = size * _canvasMultiplier;
    }

    public void ChangeEditingMode(bool isEditing) {
        _isEditing = isEditing;
    }
}
