using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ThemeSpriteRenderer : ThemeElement<SpriteRenderer> {
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
