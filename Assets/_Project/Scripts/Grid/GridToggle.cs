using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class GridToggle : MonoBehaviour {
    [SerializeField] private GridManager _gridManager;

    private Toggle _toggle;

    private void Awake() {
        _toggle = GetComponent<Toggle>();
        _gridManager.StateChanged += UpdateToggle;
        _toggle.onValueChanged.AddListener(_gridManager.ChangeStateSilently);
    }

    private void OnEnable() {
        UpdateToggle();
    }

    private void UpdateToggle() {
        _toggle.SetIsOnWithoutNotify(_gridManager.IsActive);
    }
}
