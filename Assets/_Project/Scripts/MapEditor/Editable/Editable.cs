using UnityEngine;

public abstract class Editable<TData> : BaseEditable where TData : EditableData {
    protected TData _data;

    public TData Data => _data;

    public override void SetData(EditableData data) {
        _data = data as TData;
        if (_data == null)
            Debug.LogError($"Types mismatch: expected {nameof(TData)}, got something else");

        base.SetData(data);
    }
}
