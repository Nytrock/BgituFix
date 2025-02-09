using UnityEngine;
using UnityEngine.UI;

public class EditableComputerRenderer : EditableRenderer {
    [SerializeField] private GridLayoutGroup _errors;
    [SerializeField] private float _errorsSizeMultiplier;
    [SerializeField] private float _errorsSpacingMultiplier;

    public override void SetSize(Vector2 size) {
        base.SetSize(size);
        _errors.cellSize = size * _errorsSizeMultiplier;
        _errors.padding.left = (int)_errors.cellSize.x / -2;
        _errors.padding.top = (int)_errors.cellSize.x / -2;
        _errors.spacing = new Vector2(_errorsSpacingMultiplier * size.x, 0);
    }
}
