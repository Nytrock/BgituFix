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
    public Vector2 PositionVector => _positionVector;
    public Vector2 SizeVector => _sizeVector;
    public string Position => position;
    public string Size => size;

    public EditableData(Vector2 position, Vector2 size) {
        _positionVector = position;
        this.position = position.VectorToString();

        _sizeVector = size;
        this.size = size.VectorToString();
    }

    public void UpdatePosition(Vector2 position) {
        _positionVector = position;
        this.position = _positionVector.VectorToString();
    }

    public void UpdateSize(float width, float heigth) {
        _sizeVector = new(width, heigth);
        size = _sizeVector.VectorToString();
    }

    public void SetupVectors() {
        _positionVector = position.StringToVector();
        _sizeVector = size.StringToVector();
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
