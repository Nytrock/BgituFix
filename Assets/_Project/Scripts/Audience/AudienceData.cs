using System;
using UnityEngine;

[Serializable]
public class AudienceData : EditableData {
    [SerializeField] private string name;
    [SerializeField] private int buildingId;
    [SerializeField] private int floor;
    [SerializeField] private bool isComputer;

    public string Name => name;
    public int BuildId => buildingId;
    public int Floor => floor;
    public bool IsComputer => isComputer;

    public AudienceData(int buildId, int floorIndex, int audiencesCount) : base(Vector2.zero, new(2, 2)) {
        id = -1;
        buildingId = buildId;
        floor = floorIndex;
        isComputer = false;
        name = $"Аудитория {audiencesCount + 1}";
    }

    public AudienceData(AudienceData data) : base(data._positionVector, data._sizeVector) {
        id = -1;
        buildingId = data.buildingId;
        floor = data.floor;
        isComputer = data.isComputer;
        name = data.name;
    }

    public void UpdateIsComputer(bool isComputer) {
        this.isComputer = isComputer;
    }

    public void UpdateName(string name) {
        this.name = name;
    }

    public bool Equals(AudienceData other) {
        return name == other.name && isComputer == other.isComputer && base.Equals(other);
    }
}