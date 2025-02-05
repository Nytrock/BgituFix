using System;
using UnityEngine;

[Serializable]
public class StateStyle {
    [SerializeField] private Color _backgroundColor;
    [SerializeField] private Color _textColor;

    public Color BackgroundColor => _backgroundColor;
    public Color TextColor => _textColor;
}
