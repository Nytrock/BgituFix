using System;
using UnityEngine;

[Serializable]
public class EditableData {
    [SerializeField] protected int id;
    [SerializeField] protected string position;
    [SerializeField] protected string size;

    protected Vector2 _positionVector;
    protected Vector2 _sizeVector;

    public int Id => id;
    public Vector2 Position => _positionVector;
    public Vector2 Size => _sizeVector;
    public Vector2 LeftBottom => Position - Size / 2f;
    public Vector2 RightTop => Position + Size / 2f;

    public EditableData(Vector2 position, Vector2 size) {
        _positionVector = position;
        this.position = position.ToSerializableString();

        _sizeVector = size;
        this.size = size.ToSerializableString();
    }

    public void UpdatePosition(Vector2 position) {
        _positionVector = position;
        this.position = _positionVector.ToSerializableString();
    }

    public void UpdateSize(float width, float heigth) {
        _sizeVector = new(width, heigth);
        size = _sizeVector.ToSerializableString();
    }

    public void SetupVectors() {
        _positionVector = position.ToVector();
        _sizeVector = size.ToVector();
    }

    public void SetId(int id) {
        if (this.id != -1)
            return;

        this.id = id;
    }

    public bool Equals(EditableData other) {
        return id == other.id && position == other.position && size == other.size;
    }
}
