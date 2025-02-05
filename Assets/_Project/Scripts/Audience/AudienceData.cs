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

    public AudienceData(int buildId, int floorIndex, int audiencesCount, float size) : base(Vector2.zero, new(size, size)) {
        _id = -1;
        _buildId = buildId;
        _floor = floorIndex;
        _isComputer = false;
        _name = $"Аудитория {audiencesCount + 1}";
    }

    public AudienceData(AudienceData data, float offset) : base(data._sizeVector, data._positionVector + new Vector2(offset, offset)) {
        _id = -1;
        _buildId = data._buildId;
        _floor = data._floor;
        _isComputer = data._isComputer;
        _name = data._name + " (Копия)";
        _sizeVector = data._sizeVector;
    }

    public void UpdateIsComputer(bool isComputer) {
        _isComputer = isComputer;
    }

    public void UpdateName(string name) {
        _name = name;
    }
}
