using System;
using UnityEngine;

[Serializable]
public class AudienceData : EditableData {
    [SerializeField] private string _name;
    [SerializeField] private int _buildId;
    [SerializeField] private int _floor;
    [SerializeField] private bool _isComputer;

    public string Name => _name;
    public int BuildId => _buildId;
    public int Floor => _floor;
    public bool IsComputer => _isComputer;
}
