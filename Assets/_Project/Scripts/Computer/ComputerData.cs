using System;
using UnityEngine;

[Serializable]
public class ComputerData : EditableData {
    [SerializeField] private string serialNumber;
    [SerializeField] private int auditoriumId;

    public string SerialNumber => serialNumber;
    public int AudienceId => auditoriumId;

    public ComputerData(int audienceId, int computersCount) : base(Vector2.zero, new(0.3f, 0.3f)) {
        id = -1;
        serialNumber = $"Компьютер {computersCount + 1}";
        auditoriumId = audienceId;
    }

    public ComputerData(ComputerData data) : base(data._positionVector, data._sizeVector) {
        id = -1;
        auditoriumId = data.auditoriumId;
        serialNumber = data.serialNumber;
    }

    public void UpdateNumber(string newNumber) {
        serialNumber = newNumber;
    }

    public bool Equals(ComputerData other) {
        return serialNumber == other.serialNumber && base.Equals(other);
    }
}
