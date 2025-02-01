using UnityEngine;

public class EditableAudienceActivator : EditableActivator {
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private float _canvasMultiplier;
    [SerializeField] private Color _computerColor;
    [SerializeField] private Color _nonComputerColor;

    private AudienceData _data;

    protected override void OnMouseDown() {
        if (!_data.IsComputer)
            return;

        base.OnMouseDown();
    }

    public override void Setup(EditableData data) {
        base.Setup(data);
        _canvas.sizeDelta = data.Size * _canvasMultiplier;

        _data = data as AudienceData;
        _renderer.color = _data.IsComputer ? _computerColor : _nonComputerColor;
    }
}
