using System;
using UnityEngine;

[Serializable]
public class ComputerData : EditableData {
    [SerializeField] private string _serialNumber;
    [SerializeField] private int _audienceId;

    public string SerialNumber => _serialNumber;
    public int AudienceId => _audienceId;

    public ComputerData(int audienceId, int computersCount, float size) : base(Vector2.zero, new(size, size)) {
        _id = -1;
        _serialNumber = $"Компьютер {computersCount + 1}";
        _audienceId = audienceId;
    }

    public ComputerData(ComputerData data, float offset) : base(data._sizeVector, data._positionVector + new Vector2(offset, offset)) {
        _id = -1;
        _serialNumber = data._serialNumber + " (Копия)";
        _audienceId = data._audienceId;
    }

    public void UpdateNumber(string newNumber) {
        _serialNumber = newNumber;
    }
}
