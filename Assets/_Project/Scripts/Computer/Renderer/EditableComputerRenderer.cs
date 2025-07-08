using UnityEngine;

public class EditableComputerRenderer : EditableRenderer {
    [SerializeField] private EditableComputerIcon _icon;

    public override void SetData(EditableData data) {
        base.SetData(data);
        UpdateType((data as ComputerData).Type);
    }

    public override void SetSize(Vector2 size) {
        base.SetSize(size);
        _icon.Resize(size);
    }

    public void UpdateType(ComputerType computerType) {
        _icon.UpdateType(computerType);
    }
}
