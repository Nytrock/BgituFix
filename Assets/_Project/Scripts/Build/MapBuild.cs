using System.Collections.Generic;
using UnityEngine;

public class MapBuild : MapElement<EditableAudience, AudienceData, BuildData> {
    [SerializeField] private MapBuildFloorPool _floorPool;

    private readonly List<MapBuildFloor> _floors = new();
    private int _nowFloor;

    public int FloorsCount => _data.FloorsCount;
    public override int Id => _data.Id;
    public string Name => _data.Name;

    protected override void GenerateEditables(MapData mapData) {
        GenerateFloors();
        base.GenerateEditables(mapData);
        SetupFloorsSizes();
    }

    public void SetManagers(MapManager mapManager, ErrorManager errorManager) {
        SetMapManager(mapManager);

        errorManager.ErrorAdded += CheckNewError;
        errorManager.ErrorChanged += CheckChangedError;
        errorManager.ErrorDeleted += CheckDeletedError;
    }

    private void CheckNewError(ComputerErrorData data) {
        foreach (var floor in _floors)
            floor.CheckNewError(data);
    }

    private void CheckDeletedError(ComputerErrorData data) {
        foreach (var floor in _floors)
            floor.CheckDeletedError(data);
    }

    private void CheckChangedError(ComputerErrorData data) {
        foreach (var floor in _floors)
            floor.CheckChangedError(data);
    }

    private void GenerateFloors() {
        for (int i = 0; i < _data.FloorsCount; i++)
            GenerateFloor();
        ChangeFloor(1);
    }

    private void GenerateFloor() {
        MapBuildFloor floor = _floorPool.GetObject();
        _floors.Add(floor);
        floor.ChangeState(false);
    }

    public MapBuildFloor GetFloor(int index) {
        return _floors[index - 1];
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
