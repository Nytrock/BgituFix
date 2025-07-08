using System;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : Singleton<ThemeManager> {
    [SerializeField] private ThemeColor[] _colors;
    [SerializeField] private string _fileName;

    private readonly Dictionary<ThemeColorVariable, ThemeColor> _colorsDictionary = new();
    private static FileManager _fileManager;
    private SettingsData _settings;

    public event Action IsModeChanged;

    public bool IsDarkMode => _settings.IsDarkMode;

    [ContextMenu(nameof(InitializeSingleton))]
    private void InitializeSingletonInEditor() {
        if (Application.isPlaying)
            return;
        InitializeSingleton();
    }

    protected override void Awake() {
        base.Awake();
        _fileManager = new(_fileName);
        LoadSettings();
    }

    private void Start() {
        IsModeChanged?.Invoke();
    }

    private void LoadSettings() {
        _settings = _fileManager.Load<SettingsData>();
        _settings ??= new();
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
        return _colorsDictionary[variable].GetColor(_settings.IsDarkMode);
    }

    public void ChangeMode(bool isDarkMode) {
        _settings.ChangeDarkMode(isDarkMode);
        _fileManager.Save(_settings);
        IsModeChanged?.Invoke();
    }
}
