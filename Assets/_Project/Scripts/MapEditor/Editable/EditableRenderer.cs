using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EditableRenderer : MonoBehaviour {
    [SerializeField] private int _defaultSpriteLayer;
    [SerializeField] private int _editSpriteLayer;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private EditableErrorsRendererResizer _errorsResizer;

    protected SpriteRenderer _renderer;
    protected RectTransform _canvasRect;

    public void Setup() {
        if (_renderer != null) return;

        _renderer = GetComponent<SpriteRenderer>();
        _canvasRect = _canvas.GetComponent<RectTransform>();
    }

    public virtual void SetData(EditableData data) {
        SetSize(data.Size);
    }

    public virtual void SetSize(Vector2 size) {
        _renderer.size = size;
        _canvasRect.sizeDelta = size * (1 / _canvasRect.localScale.x);
        _errorsResizer.Resize(size);
    }

    public void ChangeEditingMode(bool isEditing) {
        int order = isEditing ? _editSpriteLayer : _defaultSpriteLayer;
        _renderer.sortingOrder = order;
        _canvas.sortingOrder = order + 1;
    }
}
