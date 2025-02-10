using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditableAudienceRenderer : EditableRenderer {
    [SerializeField] private Image _errorRenderer;
    [SerializeField] private float _errorRendererMultiplier;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private float _nameTextMultiplier;
    [SerializeField] private StateStyle _computerStyle;
    [SerializeField] private StateStyle _nonComputerStyle;

    private AudienceData _data;

    public override void Setup(EditableData data) {
        base.Setup(data);

        _data = data as AudienceData;
        UpdateName();
        UpdateStyle();
    }

    public void UpdateName() {
        _nameText.text = _data.Name;
    }

    public override void SetSize(Vector2 size) {
        base.SetSize(size);
        float avgSize = (size.x + size.y) / 2f;
        _errorRenderer.rectTransform.sizeDelta = new Vector2(avgSize, avgSize) * _errorRendererMultiplier;
        _nameText.fontSize = avgSize * _nameTextMultiplier;
    }

    public void UpdateStyle() {
        StateStyle style = _nonComputerStyle;
        if (_data.IsComputer)
            style = _computerStyle;

        _renderer.color = style.BackgroundColor;
        _nameText.color = style.TextColor;
    }
}
