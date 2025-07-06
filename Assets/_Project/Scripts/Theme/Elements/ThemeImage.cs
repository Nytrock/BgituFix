using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ThemeImage : ThemeElement<Image> {
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
