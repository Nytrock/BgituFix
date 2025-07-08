using System;
using UnityEngine;

[Serializable]
public class StateStyle {
    [SerializeField] private ThemeColorVariable _backgroundColor;
    [SerializeField] private ThemeColorVariable _textColor;

    public Color BackgroundColor => ThemeManager.Instance.GetColor(_backgroundColor);
    public Color TextColor => ThemeManager.Instance.GetColor(_textColor);
}
