using System;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : Singleton<ThemeManager> {
    [SerializeField] private bool _isDarkMode;
    [SerializeField] private ThemeColor[] _colors;

    private readonly Dictionary<ThemeColorVariable, ThemeColor> _colorsDictionary = new();

    public event Action IsModeChanged;

    [ContextMenu(nameof(InitializeSingleton))]
    private void InitializeSingletonInEditor() {
        if (Application.isPlaying)
            return;
        InitializeSingleton();
    }

    private void GenerateDictionary() {
        foreach (var color in _colors)
            _colorsDictionary[color.Variable] = color;
    }

    public Color GetErrorColor(ComputerErrorType type) {
        switch (type) {
            case ComputerErrorType.Critical: return GetColor(ThemeColorVariable.CriticalError);
            case ComputerErrorType.Standard: return GetColor(ThemeColorVariable.MediumError);
            case ComputerErrorType.Minor: return GetColor(ThemeColorVariable.MinorError);
            default: return GetColor(ThemeColorVariable.Primary);
        }
    }

    public Color GetColor(ThemeColorVariable variable) {
        if (!Application.isPlaying || _colorsDictionary.Count == 0)
            GenerateDictionary();
        return _colorsDictionary[variable].GetColor(_isDarkMode);
    }

    public void ChangeMode(bool isDarkMode) {
        _isDarkMode = isDarkMode;
        IsModeChanged?.Invoke();
    }
}
