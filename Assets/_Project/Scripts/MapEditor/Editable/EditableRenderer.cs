using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EditableRenderer : MonoBehaviour {
    [SerializeField] private int _defaultSpriteLayer;
    [SerializeField] private int _editSpriteLayer;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _infoPanel;
    [SerializeField] private float _infoPanelMultiplier;

    protected SpriteRenderer _renderer;
    protected RectTransform _canvasRect;

    private void GetComponents() {
        if (_renderer != null) return;

        _renderer = GetComponent<SpriteRenderer>();
        _canvasRect = _canvas.GetComponent<RectTransform>();
    }

    public virtual void Setup(EditableData data) {
        GetComponents();
        SetSize(data.SizeVector);
    }

    public virtual void SetSize(Vector2 size) {
        _renderer.size = size;
        _canvasRect.sizeDelta = size * (1 / _canvasRect.localScale.x);

        float infoSize = (size.x + size.y) / 2f * _infoPanelMultiplier;
        _infoPanel.localScale = new(infoSize, infoSize);
    }

    public void ChangeEditingMode(bool isEditing) {
        int order = isEditing ? _editSpriteLayer : _defaultSpriteLayer;
        _renderer.sortingOrder = order;
        _canvas.sortingOrder = order + 1;
    }
}
