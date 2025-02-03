using System;
using UnityEngine;

[Serializable]
public class EditableData {
    [SerializeField] private int _id;
    [SerializeField] private Vector2 _position;
    [SerializeField] private Vector2 _size;

    public int Id => _id;
    public Vector2 Position => _position;
    public Vector2 Size => _size;

    public void UpdatePosition(Vector2 position) {
        _position = position;
    }

    public void UpdateSize(float width, float heigth) {
        _size = new(width, heigth);
    }
}
