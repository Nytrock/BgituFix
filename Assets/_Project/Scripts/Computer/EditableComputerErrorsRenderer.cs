using System;
using UnityEngine;

public class EditableComputerErrorsRenderer : MonoBehaviour {
    [SerializeField] private ComputerErrorRendererWithCount[] _errorRenderers;

    public void Setup() {
        int enumCount = Enum.GetValues(typeof(ComputerErrorType)).Length - 1;
        for (int i = 0; i < enumCount; i++) {
            _errorRenderers[i].SetVisual((ComputerErrorType)i);
            _errorRenderers[i].ChangeState(false);
        }
    }

    public void AddError(ComputerErrorData errorData) {
        int index = (int)errorData.Type;
        _errorRenderers[index].ChangeState(true);
        _errorRenderers[index].AddToCount();
    }
}
