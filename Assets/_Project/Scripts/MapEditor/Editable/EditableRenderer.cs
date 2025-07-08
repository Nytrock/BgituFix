using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EditableRenderer : MonoBehaviour {
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
}
