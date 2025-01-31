using UnityEngine;

public class EditableAudienceActivator : EditableActivator {
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private float _canvasMultiplier;

    public override void Setup(EditableData data) {
        base.Setup(data);
        _canvas.sizeDelta = data.Size * _canvasMultiplier;
    }
}
