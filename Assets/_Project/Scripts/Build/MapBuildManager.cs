using System;
using UnityEngine;

public class MapBuildManager : MapElementManager<MapBuild, EditableAudience, AudienceData, BuildData> {

    [SerializeField] protected MapBuildPool _buildPool;

    public event Action<MapBuild> BuildAdded;
    public event Action<MapBuild> BuildChanged;

    public void GenerateBuilds(MapData mapData) {
        foreach (var buildData in mapData.BuildDatas)
            GenerateBuild(mapData, buildData);

        SelectElement(_mapElements[0]);
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
                SelectElement(build);
                break;
            }
        }
    }

    public override void ChangeState(bool newState) {
        base.ChangeState(newState);
        if (newState)
            SelectElement(_mapElements[0]);
    }

    protected override void SelectElement(MapBuild build) {
        base.SelectElement(build);
        BuildChanged?.Invoke(build);
    }
}
