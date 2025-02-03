using System;
using UnityEngine;

public class MapBuildManager : MapElementManager<MapBuild, EditableAudience, AudienceData, BuildData> {

    [SerializeField] protected MapBuildPool _buildPool;

    public event Action<MapBuild> BuildAdded;
    public event Action<MapBuild> BuildChanged;

    public void GenerateBuilds(MapManager mapManager) {
        MapData mapData = mapManager.Data;
        foreach (var buildData in mapData.BuildDatas)
            GenerateBuild(mapData, buildData);

        UpdateElement(_mapElements[0]);
    }

    private void GenerateBuild(MapData mapData, BuildData buildData) {
        MapBuild build = _buildPool.GetObject();
        build.Setup(mapData, buildData);
        _mapElements.Add(build);
        BuildAdded?.Invoke(build);
    }

    public void ChangeBuild(int id) {
        foreach (var build in _mapElements) {
            if (build.Id == id) {
                UpdateElement(build);
                break;
            }
        }
    }

    public override void ChangeState(bool newState) {
        base.ChangeState(newState);
        if (newState)
            UpdateElement(_mapElements[0]);
    }

    protected override void UpdateElement(MapBuild build) {
        base.UpdateElement(build);
        BuildChanged?.Invoke(build);
    }
}
