using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ThemeText : ThemeElement<TextMeshProUGUI> {
    [ContextMenu(nameof(UpdateColor))]
    public void UpdateColor() {
        if (!Application.isPlaying)
            GetComponents();
        SetColor();
    }

    protected override void SetColor() {
        _element.color = ThemeManager.Instance.GetColor(_variable);
    }
}
