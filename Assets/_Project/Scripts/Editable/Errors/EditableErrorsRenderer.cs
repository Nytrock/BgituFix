using System;
using UnityEngine;

public class EditableErrorsRenderer : MonoBehaviour {
    [SerializeField] private ComputerErrorRendererWithCount[] _errorRenderers;

    private bool _isEmpty;
    private bool _isEdit;

    public void Setup() {
        int enumCount = Enum.GetValues(typeof(ComputerErrorType)).Length - 1;
        for (int i = 0; i < enumCount; i++) {
            _errorRenderers[i].SetType((ComputerErrorType)(i + 1));
            _errorRenderers[i].UpdateState();
        }
        CheckEmpty();
    }

    public void AddError(ComputerErrorData errorData) {
        int index = (int)(errorData.Type - 1);
        _errorRenderers[index].AddToCount();
        CheckEmpty();
    }

    private void CheckEmpty() {
        _isEmpty = GetMaxError() == ComputerErrorType.None;
        UpdateState();
    }

    public void RemoveError(ComputerErrorData errorData) {
        int index = (int)(errorData.Type - 1);
        _errorRenderers[index].RemoveFromCount();
        CheckEmpty();
    }

    public void ChangeEditState(bool isEdit) {
        _isEdit = isEdit;
        UpdateState();
    }

    private void UpdateState() {
        gameObject.SetActive(!_isEdit && !_isEmpty);
    }

    public ComputerErrorType GetMaxError() {
        int enumCount = Enum.GetValues(typeof(ComputerErrorType)).Length - 1;
        for (int i = enumCount - 1; i >= 0; i--)
            if (_errorRenderers[i].Count != 0)
                return _errorRenderers[i].Type;

        return ComputerErrorType.None;
    }
}
