using UnityEngine;
using UnityEngine.UI;

public class EditableComputerIcon : MonoBehaviour {
    [SerializeField] private Image _image;
    [SerializeField] private float _sizeMultiplier;
    [SerializeField] private EditableComputerTypeIcon[] _icons;

    public void UpdateType(ComputerType computerType) {
        foreach (var icon in _icons) {
            if (icon.Type == computerType) {
                _image.sprite = icon.Icon;
                break;
            }
        }
    }

    public void Resize(Vector2 size) {
        _image.rectTransform.sizeDelta = size * _sizeMultiplier;
    }
}
