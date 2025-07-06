using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ThemeCamera : ThemeElement<Camera> {
    [ContextMenu(nameof(UpdateColor))]
    public void UpdateColor() {
        if (!Application.isPlaying)
            GetComponents();
        SetColor();
    }

    protected override void SetColor() {
        _element.backgroundColor = ThemeManager.Instance.GetColor(_variable);
    }
}
