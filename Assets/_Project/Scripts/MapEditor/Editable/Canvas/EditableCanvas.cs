using TMPro;
using UnityEngine;

public class EditableCanvas : MonoBehaviour {
    [SerializeField] private GameObject _panel;
    [SerializeField] private EditableCanvasInfo _infoPanel;
    [SerializeField] private GameObject _border;
    [SerializeField] private EditableCanvasBorderElement[] _borderElements;
    [SerializeField] private TextMeshProUGUI _widthText;
    [SerializeField] private TextMeshProUGUI _lengthText;

    private BaseEditable _editable;
    private bool _isResizing;

    public bool IsInfoOpen => _infoPanel.IsActive;

    private void Awake() {
        ChangeInfoState(false);
        ChangeBorderState(false);
    }

    private void Update() {
        if (!_isResizing)
            return;

        _widthText.text = _editable.Size.x.ToString() + Units.SIZE_UNIT;
        _lengthText.text = _editable.Size.y.ToString() + Units.SIZE_UNIT;
    }

    public void SetEditable(BaseEditable editable) {
        _editable = editable;
        _infoPanel.SetEditable(editable);
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
        _infoPanel.ChangeState(false);
    }

    public void ChangeInfoState() {
        _infoPanel.ChangeState();
    }

    public void ChangeInfoState(bool newState) {
        _infoPanel.ChangeState(newState);
    }

    public void ChangeBorderState(bool isEditing) {
        _border.SetActive(isEditing);
        _isResizing = isEditing;
    }
}
