using System;
using UnityEngine;

[RequireComponent(typeof(CustomDropdown))]
public class ThemeDropdown : MonoBehaviour {
    [SerializeField] private ThemeManager _themeManager;

    private CustomDropdown _dropdown;

    private void Awake() {
        _dropdown = GetComponent<CustomDropdown>();
        _dropdown.onValueChanged.AddListener(ChangeTheme);
        SetupDropdown();
    }

    private void ChangeTheme(int value) {
        _themeManager.ChangeMode(Convert.ToBoolean(value));
    }

    private void SetupDropdown() {
        _dropdown.SetValueWithoutNotify(Convert.ToInt32(_themeManager.IsDarkMode));
    }
}
