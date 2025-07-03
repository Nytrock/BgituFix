using UnityEngine;

public class SelectManager : MonoBehaviour {
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private MapEditManager _editManager;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private Selector _selector;

    private BaseEditable _editable;
    private bool _leftWasHeldBeforeEditable;
    private bool _rightWasHeldBeforeEditable;

    public bool IsSelectorActive => _selector.IsActive;

    private void Awake() {
        _mapManager.MapLocationChanged += ResetEditable;
    }

    public void SetEditable(BaseEditable editable) {
        _leftWasHeldBeforeEditable = Input.GetMouseButton(0);
        _rightWasHeldBeforeEditable = Input.GetMouseButton(1);
        _editable = editable;
    }

    public void ResetEditable() {
        _editable = null;
    }

    private void Update() {
        if (Input.GetMouseButtonUp(0))
            LeftButtonUp();

        if (Input.GetMouseButtonUp(1))
            RightButtonUp();

        if (_cameraManager.IsHover)
            return;

        if (Input.GetMouseButtonDown(0))
            LeftButtonDown();
    }

    private void LeftButtonUp() {
        if (_leftWasHeldBeforeEditable) {
            _leftWasHeldBeforeEditable = false;
            return;
        }

        if (_editable == null) {
            _selector.ChangeState(false);
            return;
        }

        _editable.LeftButtonUp();
    }

    private void RightButtonUp() {
        if (_rightWasHeldBeforeEditable) {
            _rightWasHeldBeforeEditable = false;
            return;
        }

        if (_editable == null || _selector.IsActive)
            return;

        _editable.RightButtonUp();
    }

    private void LeftButtonDown() {
        if (_editable != null) {
            _editable.LeftButtonDown();
            return;
        }

        if (!_editManager.IsEdit)
            return;

        _editManager.DeselectAllNowEditables();
        _selector.ChangeState(true);
    }
}
