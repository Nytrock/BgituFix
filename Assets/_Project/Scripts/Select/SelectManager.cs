using UnityEngine;

public class SelectManager : MonoBehaviour {
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private CameraManager _cameraManager;

    private BaseEditable _editable;

    private bool _buttonWasHeldBeforeEditable;
    private bool _onHoverWasOnDown;

    private void Awake() {
        _mapManager.MapLocationChanged += ResetEditable;
    }

    public void SetEditable(BaseEditable editable, bool checkButtonHeld = true) {
        if (checkButtonHeld)
            _buttonWasHeldBeforeEditable = Input.GetMouseButton(0);
        _editable = editable;
    }

    public void ResetEditable() {
        _editable = null;
    }

    private void Update() {
        if (_onHoverWasOnDown) {
            if (!_cameraManager.IsHover)
                _onHoverWasOnDown = false;
            return;
        }

        if (Input.GetMouseButtonUp(0))
            LeftButtonUp();

        if (Input.GetMouseButtonDown(0))
            LeftButtonDown();
    }

    private void LeftButtonUp() {
        if (_buttonWasHeldBeforeEditable) {
            _buttonWasHeldBeforeEditable = false;
            return;
        }

        if (_editable == null)
            return;

        _editable.LeftButtonUp();
    }

    private void LeftButtonDown() {
        _onHoverWasOnDown = _cameraManager.IsHover;
        if (_onHoverWasOnDown)
            return;

        if (_editable != null) {
            _editable.LeftButtonDown();
            return;
        }
    }
}
