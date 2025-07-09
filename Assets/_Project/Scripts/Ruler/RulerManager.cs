using System;
using UnityEngine;

public class RulerManager : MonoBehaviour {
    [SerializeField] private RulerElement _ruler;

    private bool _isActive;

    public bool IsActive => _isActive;

    public event Action StateChanged;

    public void ChangeState(bool newState) {
        ChangeStateSilently(newState);
        StateChanged?.Invoke();
    }

    public void ChangeStateSilently(bool newState) {
        _isActive = newState;
        _ruler.ChangeState(newState);
        CursorManager.Instance.SetType(_isActive ? CursorType.Ruler : CursorType.Default);
    }
}
