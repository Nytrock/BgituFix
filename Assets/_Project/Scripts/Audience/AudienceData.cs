using System;
using UnityEngine;

[Serializable]
public class AudienceData : EditableData {
    [SerializeField] private string _name;
    [SerializeField] private int _buildId;
    [SerializeField] private int _floor;
    [SerializeField] private bool _isDisabled;
    [SerializeField] private float _width;
    [SerializeField] private float _length;

    public string Name => _name;
    public int BuildId => _buildId;
    public int Floor => _floor;
    public bool IsDisabled => _isDisabled;
    public float Width => _width;
    public float Length => _length;
}
