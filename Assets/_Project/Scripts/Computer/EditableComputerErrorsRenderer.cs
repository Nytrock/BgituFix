using System;
using UnityEngine;

public class EditableComputerErrorsRenderer : MonoBehaviour {
    [SerializeField] private ComputerErrorRendererWithCount[] _errorRenderers;

    public void Setup() {
        int enumCount = Enum.GetValues(typeof(ComputerErrorType)).Length - 1;
        for (int i = 0; i < enumCount; i++) {
            _errorRenderers[i].SetType((ComputerErrorType)(i + 1));
            _errorRenderers[i].ChangeState(false);
        }
    }

    public void AddError(ComputerErrorData errorData) {
        int index = (int)(errorData.Type - 1);
        _errorRenderers[index].AddToCount();
    }

    public void RemoveError(ComputerErrorData errorData) {
        int index = (int)(errorData.Type - 1);
        _errorRenderers[index].RemoveFromCount();
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }
}
