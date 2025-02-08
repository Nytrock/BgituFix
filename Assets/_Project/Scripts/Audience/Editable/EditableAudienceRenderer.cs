using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditableAudienceRenderer : EditableRenderer {
    [SerializeField] private Image _errorRenderer;
    [SerializeField] private float _errorRendererMultiplier;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private StateStyle _computerStyle;
    [SerializeField] private StateStyle _nonComputerStyle;

    private AudienceData _data;

    public override void Setup(EditableData data) {
        base.Setup(data);

        _data = data as AudienceData;
        UpdateName();
        UpdateStyle();
    }

    public override void SetSize(Vector2 size) {
        base.SetSize(size);
        _errorRenderer.rectTransform.sizeDelta = size * _errorRendererMultiplier;
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
