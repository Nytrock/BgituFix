using UnityEngine;
using UnityEngine.UI;

public class RulerToggle : MonoBehaviour {
    [SerializeField] private RulerManager _rulerManager;

    private Toggle _toggle;

    private void Awake() {
        _toggle = GetComponent<Toggle>();
        _rulerManager.StateChanged += UpdateToggle;
        _toggle.onValueChanged.AddListener(_rulerManager.ChangeStateSilently);
    }

    private void UpdateToggle() {
        _toggle.SetIsOnWithoutNotify(_rulerManager.IsActive);
    }
}
