using UnityEngine;

public class EditableCanvasInfo : MonoBehaviour {
    protected BaseEditable _editable;
    protected bool _isActive;

    public bool IsActive => _isActive;

    public virtual void SetEditable(BaseEditable editable) {
        _editable = editable;
    }

    public void ChangeState() {
        _isActive = !_isActive;
        UpdateState();
    }

    public void ChangeState(bool newState) {
        _isActive = newState;
        UpdateState();
    }

    protected virtual void UpdateState() {
        gameObject.SetActive(_isActive);
    }
}
