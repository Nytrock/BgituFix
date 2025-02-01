using System.Collections.Generic;
using UnityEngine;

public class MapBuild : MapElement<EditableAudience, AudienceData, BuildData> {
    [SerializeField] private MapBuildFloorPool _floorPool;

    private MapBuildFloor[] _floors;
    private int _nowFloor;

    public int FloorsCount => _data.FloorsCount;
    public override int Id => _data.Id;
    public string Name => _data.Name;

    protected override void GenerateEditables(MapData mapData) {
        GenerateFloors();
        base.GenerateEditables(mapData);
        SetupFloorsSizes();
    }

    private void GenerateFloors() {
        _floors = new MapBuildFloor[_data.FloorsCount];
        for (int i = 0; i < _data.FloorsCount; i++) {
            MapBuildFloor floor = _floorPool.GetObject();
            _floors[i] = floor;
            floor.ChangeState(false);
        }
        ChangeFloor(1);
    }

    private void SetupFloorsSizes() {
        _cameraSize = 0;
        foreach (var floor in _floors) {
            floor.SetupSize();
            if (floor.CameraSize > _cameraSize)
                _cameraSize = floor.CameraSize;
        }
    }

    public void ChangeFloor(int floor) {
        if (floor < 0 || floor > _data.FloorsCount)
            return;

        if (_nowFloor != 0)
            _floors[_nowFloor - 1].ChangeState(false);
        _nowFloor = floor;
        _floors[_nowFloor - 1].ChangeState(true);
    }

    protected override EditableAudience GenerateEditable(AudienceData data) {
        EditableAudience audience = base.GenerateEditable(data);
        _floors[data.Floor - 1].AddAudience(audience);
        return audience;
    }

    protected override IEnumerable<AudienceData> GetEditablesData(MapData mapData) {
        return mapData.GetAudiencesByBuild(_data);
    }
}
