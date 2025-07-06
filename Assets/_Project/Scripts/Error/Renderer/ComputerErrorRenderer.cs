using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ComputerErrorRenderer : MonoBehaviour {
    private Image _image;
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
        ChangeState(_type != ComputerErrorType.None);
        if (_type == ComputerErrorType.None)
            return;

        CheckImage();
        _image.color = ThemeManager.Instance.GetErrorColor(type);
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }
}
