using System;
using UnityEngine;

public class StateMachine : MonoBehaviour {
    [SerializeField] protected GameObject _panel;

    public event Action<bool> StateChanged;

    public virtual void ChangeState(bool newState) {
        _panel.SetActive(newState);
        StateChanged?.Invoke(newState);
    }

    public void ChangeState() {
        _panel.SetActive(!_panel.activeSelf);
    }
}
