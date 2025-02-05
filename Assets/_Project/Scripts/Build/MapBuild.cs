using System;
using System.Collections.Generic;
using UnityEngine;

public class MapBuild : MapElement<EditableAudience, AudienceData, BuildData> {
    [SerializeField] private MapBuildFloorPool _floorPool;

    private readonly List<MapBuildFloor> _floors = new();
    private int _nowFloor;

    public int FloorsCount => _data.FloorsCount;
    public override int Id => _data.Id;
    public string Name => _data.Name;
    public int NowFloor => _nowFloor;

    public event Action<int> FloorChanged;

    protected override void GenerateEditables(MapData mapData) {
        GenerateFloors();
        base.GenerateEditables(mapData);
        SetupFloorsSizes();
    }

    protected override void ChangeSizeShowState(bool newState) {
        _floors[_nowFloor - 1].ChangeSizeShowState(newState);
    }

    public void SetManagers(MapManager mapManager, MapEditManager editManager,
        ErrorManager errorManager, UserManager userManager) {

        (_pool as EditableAudiencePool).SetManagers(mapManager, editManager);
        editManager.EditableChanged += UpdateShowingSize;
        editManager.EditStateChanged += UpdateShowingSize;
        if (userManager.ClientType != UserType.Admin)
            return;

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
        floor.ChangeSizeShowState(false);
        _floors.Add(floor);
        floor.ChangeState(false);
    }

    public MapBuildFloor GetFloor(int index) {
        return _floors[index - 1];
    }

    private void SetupFloorsSizes() {
        _cameraSize = 0;
        for (int i = 0; i < _data.FloorsCount; i++)
            SetupFloorSize(i);
    }

    private void SetupFloorSize(int index) {
        _floors[index].SetupSize();
        if (_floors[index].CameraSize > _cameraSize)
            _cameraSize = _floors[index].CameraSize;
    }

    public void ChangeFloor(int floor) {
        if (floor < 0 || floor > _data.FloorsCount)
            return;

        if (_nowFloor != 0)
            _floors[_nowFloor - 1].ChangeState(false);
        _nowFloor = floor;
        _floors[_nowFloor - 1].ChangeState(true);
        FloorChanged?.Invoke(_nowFloor);
    }

    public override EditableAudience GenerateEditable(AudienceData data) {
        EditableAudience audience = base.GenerateEditable(data);
        _floors[data.Floor - 1].AddAudience(audience);
        audience.SizeOrPositionChanged += delegate {
            SetupFloorSize(audience.Floor - 1);
        };
        return audience;
    }

    protected override IEnumerable<AudienceData> GetEditablesData(MapData mapData) {
        return mapData.GetAudiencesByBuild(_data);
    }
}
