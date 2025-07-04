using UnityEngine;

public class HelpManager : MonoBehaviour {
    [SerializeField] private GameObject _panel;

    private bool _isActive;
    public bool IsActive => _isActive;

    public void ChangeState(bool newState) {
        _isActive = newState;
        _panel.SetActive(newState);
    }
}
