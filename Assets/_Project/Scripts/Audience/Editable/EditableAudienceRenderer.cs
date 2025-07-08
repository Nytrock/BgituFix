using UnityEngine;

public class EditableAudienceRenderer : EditableRenderer {
    [SerializeField] private EditableAudienceCenterGraphic _centerGraphic;
    [SerializeField] private StateStyle _computerStyle;
    [SerializeField] private StateStyle _nonComputerStyle;

    private AudienceData _data;

    public override void SetData(EditableData data) {
        base.SetData(data);

        _data = data as AudienceData;
        UpdateName();
        UpdateStyle();
    }

    public override void SetSize(Vector2 size) {
        base.SetSize(size);
        _centerGraphic.ChangeSize(size);
    }

    public void UpdateName() {
        _centerGraphic.SetText(_data.Name);
    }

    public void UpdateStyle() {
        StateStyle style = _nonComputerStyle;
        if (_data.Type == AudienceType.Computer)
            style = _computerStyle;

        _renderer.color = style.BackgroundColor;
        _centerGraphic.SetStyle(_data.Type, style.TextColor);
    }
}
