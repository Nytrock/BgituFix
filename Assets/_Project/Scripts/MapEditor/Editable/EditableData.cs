using System;
using UnityEngine;

[Serializable]
public class EditableData {
    [SerializeField] private int _id;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Vector3 _size;

    public int Id => _id;
    public Vector3 Position => _position;
    public Vector3 Size => _size;
}
