using System;
using UnityEngine;

[RequireComponent(typeof(ButtonWithText))]
public class ComputerErrorUISolveButton : MonoBehaviour {
    [SerializeField] private StateStyle _isSolvedStyle;
    [SerializeField] private StateStyle _notSolvedStyle;
    [SerializeField] private string _isSolvedText;
    [SerializeField] private string _notSolvedText;

    private bool _isSolved;
    private ButtonWithText _button;

    public event Action<bool> OnValueChanged;

    public void SetupButton() {
        _button = GetComponent<ButtonWithText>();
        _button.onClick.AddListener(ChangeIsSolved);
    }

    public void Setup(ComputerErrorData error, UserType clientType) {
        _isSolved = error.IsSolved;
        _button.interactable = clientType == UserType.Admin;
        UpdateStyle();
    }

    public void SetIsSolvedWithoutNotify(bool isSolved) {
        _isSolved = isSolved;
        UpdateStyle();
    }

    private void ChangeIsSolved() {
        OnValueChanged?.Invoke(!_isSolved);
    }

    private void UpdateStyle() {
        StateStyle style = _isSolved ? _isSolvedStyle : _notSolvedStyle;
        string text = _isSolved ? _isSolvedText : _notSolvedText;
        _button.SetStyle(style);
        _button.SetText(text);
    }
}
