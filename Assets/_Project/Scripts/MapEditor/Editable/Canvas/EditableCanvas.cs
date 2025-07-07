using UnityEngine;

public class EditableCanvas : MonoBehaviour {
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _border;
    [SerializeField] private EditableCanvasBorderElement[] _borderElements;
    [SerializeField] private EditableTextSize _widthText;
    [SerializeField] private EditableTextSize _lengthText;

    private SelectManager _selectManager;
    private BaseEditable _editable;
    private bool _isResizing;

    private void Awake() {
        ChangeBorderState(false);
    }

    private void Update() {
        if (!_isResizing)
            return;

        _widthText.SetSize(_editable.Size.x);
        _lengthText.SetSize(_editable.Size.y);
    }

    public void Setup(BaseEditable editable, SelectManager selectManager) {
        _editable = editable;
        _selectManager = selectManager;
        SetupBorderElements();
    }

    private void SetupBorderElements() {
        foreach (var borderElement in _borderElements) {
            borderElement.CursorTypeChanged += ChangeResizeSettings;
            borderElement.LeftButtonClick += delegate {
                _selectManager.SetEditable(_editable, false);
            };
        }
    }

    public void ChangeResizeSettings(CursorType type) {
        if (_editable.IsResizing || _selectManager.IsSelectorActive)
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
    }

    public void ChangeBorderState(bool isEditing) {
        _border.SetActive(isEditing);
        _isResizing = isEditing;
    }

    public void StopResizing() {
        foreach (var borderElement in _borderElements) {
            if (borderElement.IsHover) {
                ChangeResizeSettings(borderElement.CursorType);
                return;
            }
        }

        ChangeResizeSettings(CursorType.Default);
    }
}
