using System;
using UnityEngine;

[Serializable]
public class ThemeColor {
    [SerializeField] private ThemeColorVariable _variable;
    [SerializeField] private Color _lightColor;
    [SerializeField] private Color _darkColor;

    public ThemeColorVariable Variable => _variable;

    public Color GetColor(bool isDarkMode) {
        return isDarkMode ? _darkColor : _lightColor;
    }
}
