using System;
using UnityEngine;

[Serializable]
public class SettingsData {
    [SerializeField] private bool _isDarkMode;

    public bool IsDarkMode => _isDarkMode;

    public SettingsData() {
        _isDarkMode = true;
    }

    public void ChangeDarkMode(bool isDarkMode) {
        _isDarkMode = isDarkMode;
    }
}
