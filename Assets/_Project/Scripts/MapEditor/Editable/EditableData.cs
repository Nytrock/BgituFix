using System;
using UnityEngine;

[Serializable]
public class EditableData {
    [SerializeField] protected int _id;
    [SerializeField] protected string _position;
    [SerializeField] protected string _size;

    [SerializeField] protected Vector2 _positionVector;
    [SerializeField] protected Vector2 _sizeVector;

    public int Id => _id;
    public Vector2 Position => _positionVector;
    public Vector2 Size => _sizeVector;

    public EditableData(Vector2 position, Vector2 size) {
        _positionVector = position;
        _position = position.VectorToString();

        _sizeVector = size;
        _size = size.VectorToString();
    }

    public void UpdatePosition(Vector2 position) {
        _positionVector = position;
        _position = _positionVector.VectorToString();
    }

    public void UpdateSize(float width, float heigth) {
        _sizeVector = new(width, heigth);
        _size = _sizeVector.VectorToString();
    }

    public void SetupVectors() {
        _positionVector = _position.StringToVector();
        _sizeVector = _size.StringToVector();
    }
}
