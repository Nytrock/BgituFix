using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ThemeLineRenderer : ThemeElement<LineRenderer> {
    [ContextMenu(nameof(UpdateColor))]
    public void UpdateColor() {
        if (!Application.isPlaying)
            GetComponents();
        SetColor();
    }

    protected override void SetColor() {
        Color color = ThemeManager.Instance.GetColor(_variable);
        color.a /= 2;

        _element.startColor = color;
        _element.endColor = color;
    }
}
