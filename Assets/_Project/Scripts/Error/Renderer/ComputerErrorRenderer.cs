using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ComputerErrorRenderer : MonoBehaviour {
    [SerializeField] private ComputerErrorTypeStyle[] _styles;

    private Image _image;
    private bool _isActive;
    private ComputerErrorType _type;

    public ComputerErrorType Type => _type;

    private void Awake() {
        CheckImage();
    }

    private void CheckImage() {
        if (_image != null) return;

        _image = GetComponent<Image>();
    }

    public virtual void SetType(ComputerErrorType type) {
        _type = type;
        if (_type == ComputerErrorType.None) {
            ChangeState(false);
            return;
        }

        foreach (var style in _styles)
            if (style.ErrorType == type)
                SetStyle(style);
    }

    private void SetStyle(ComputerErrorTypeStyle style) {
        CheckImage();
        _image.color = style.Color;
    }

    public void ChangeState(bool newState) {
        _isActive = newState;
        gameObject.SetActive(newState);
    }
}

[Serializable]
public class ComputerErrorTypeStyle {
    [SerializeField] private ComputerErrorType _errorType;
    [SerializeField] private Color _color;

    public ComputerErrorType ErrorType => _errorType;
    public Color Color => _color;
}
