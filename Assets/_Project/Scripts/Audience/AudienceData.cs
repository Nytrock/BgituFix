using System;
using UnityEngine;

[Serializable]
public class AudienceData : EditableData {
    [SerializeField] private string _name;
    [SerializeField] private int _buildId;
    [SerializeField] private int _floor;
    [SerializeField] private bool _isDisabled;

    public string Name => _name;
    public int BuildId => _buildId;
    public int Floor => _floor;
    public bool IsDisabled => _isDisabled;
}
