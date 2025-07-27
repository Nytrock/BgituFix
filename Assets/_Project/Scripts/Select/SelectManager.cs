using UnityEngine;

public class SelectManager : MonoBehaviour {
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private MapEditManager _editManager;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private Selector _selector;

    private ISelectable _selected;

    public bool IsSelectorActive => _selector.IsActive;

    private void Update() {
        if (_editManager.IsRulerActive)
            return;

        if (Input.GetMouseButtonUp(0))
            MouseUp();

        if (Input.GetMouseButtonDown(0))
            MouseDown();
    }

    private void MouseUp() {
        if (_selector.IsActive) {
            _selector.ChangeState(false);
            return;
        }

        if (_selected != null && _editManager.IsEdit) {
            _selected.MouseUp();
            return;
        }

        GetSelectable();
        if (_selected is null)
            return;
        _selected.MouseUp();
    }

    private void MouseDown() {
        if (_cameraManager.IsHover)
            return;

        GetSelectable();
        if (_selected is not null) {
            _selected.MouseDown();
            return;
        }

        if (!_editManager.IsEdit)
            return;

        _editManager.DeselectAllNowEditables();
        _selector.ChangeState(true);
    }

    private void GetSelectable() {
        _selected = null;
        RaycastHit2D hit = Physics2D.Raycast(CameraManager.LocalMousePosition, Vector2.zero);
        if (!hit || _cameraManager.IsHover)
            return;

        hit.collider.TryGetComponent(out _selected);
    }
}
