using TMPro;
using UnityEngine;

public class EditableAudienceRenderer : EditableRenderer {
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private float _nameTextMultiplier;
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
        float avgSize = (size.x + size.y) / 2f;
        _nameText.fontSize = avgSize * _nameTextMultiplier;
    }

    public void UpdateName() {
        _nameText.text = _data.Name;
    }

    public void UpdateStyle() {
        StateStyle style = _nonComputerStyle;
        if (_data.IsComputer)
            style = _computerStyle;

        _renderer.color = style.BackgroundColor;
        _nameText.color = style.TextColor;
    }
}
