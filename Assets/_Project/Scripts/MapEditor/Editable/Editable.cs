using UnityEngine;

public abstract class Editable<TData> : MonoBehaviour
    where TData : EditableData {
    [SerializeField] private EditableActivator _activator;
    protected TData _data;

    public Vector2 Size => _data.Size;
    public Vector2 Position => _data.Position;

    public abstract void Press();

    public virtual void Setup(TData data) {
        _data = data;
        transform.position = _data.Position;

        _activator.Setup(_data);
        _activator.Pressed += Press;
    }
}
