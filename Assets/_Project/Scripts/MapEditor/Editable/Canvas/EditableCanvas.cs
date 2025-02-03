using UnityEngine;

public class EditableCanvas : MonoBehaviour {
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _border;
    [SerializeField] private BaseEditable _editable;
    [SerializeField] private EditableCanvasBorderElement[] _borderElements;

    private void Awake() {
        ChangeInfoState(false);
        ChangeBorderState(false);
        SetupBorderElements();
    }

    private void SetupBorderElements() {
        foreach (var borderElement in _borderElements) {
            borderElement.CursorTypeChanged += ChangeResizeSettings;
            borderElement.LeftButtonClick += _editable.LeftButtonDown;
        }
    }

    public void ChangeResizeSettings(CursorType type) {
        if (_editable.IsResizing)
            return;

        CursorManager.Instance.SetType(type);
        switch (type) {
            case CursorType.Default:
                _editable.SetResizings(false, false);
                break;
            case CursorType.HorizontalResize:
                _editable.SetResizings(false, true);
                break;
            case CursorType.VerticalResize:
                _editable.SetResizings(true, false);
                break;
            case CursorType.DiagonalResizeLeftDown:
            case CursorType.DiagonalResizeLeftUp:
                _editable.SetResizings(true, true);
                break;
        }
    }

    public void ChangeState(bool newState) {
        _panel.SetActive(newState);
        _infoPanel.SetActive(false);
    }

    public void ChangeInfoState() {
        _infoPanel.SetActive(!_infoPanel.activeSelf);
    }

    public void ChangeInfoState(bool newState) {
        _infoPanel.SetActive(newState);
    }

    public void ChangeBorderState(bool isEditing) {
        _border.SetActive(isEditing);
    }
}
