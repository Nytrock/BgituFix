using System;
using UnityEngine;

[Serializable]
public class AudienceData : EditableData {
    [SerializeField] private string name;
    [SerializeField] private int buildingId;
    [SerializeField] private int floor;
    [SerializeField] private bool isComputer;
    [SerializeField] private bool isStairs;

    private AudienceType _type;

    public string Name => name;
    public int BuildId => buildingId;
    public int Floor => floor;

    public AudienceType Type {
        get {
            if (_type == AudienceType.None)
                SetupType();
            return _type;
        }
    }

    private void SetupType() {
        if (isComputer)
            _type = AudienceType.Computer;
        else if (isStairs)
            _type = AudienceType.Stairs;
        else
            _type = AudienceType.Normal;
    }

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

    public void UpdateType(AudienceType type) {
        isComputer = type == AudienceType.Computer;
        isStairs = type == AudienceType.Stairs;
        _type = type;
    }

    public void UpdateName(string name) {
        this.name = name;
    }

    public void ChangeBuild(int nowBuild, int nowFloor) {
        buildingId = nowBuild;
        floor = nowFloor;
    }

    public override bool Equals(EditableData other) {
        if (other is not AudienceData otherAudience)
            return base.Equals(other);
        return Equals(otherAudience);
    }

    public bool Equals(AudienceData other) {
        return name == other.name && isComputer == other.isComputer && isStairs == other.isStairs && base.Equals(other);
    }
}