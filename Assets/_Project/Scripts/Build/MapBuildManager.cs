using System;
using System.Collections.Generic;
using UnityEngine;

public class MapBuildManager : MonoBehaviour {
    [SerializeField] private MapBuildPool _buildPool;
    [SerializeField] private float _cameraSizeOffset;

    private readonly List<MapBuild> _builds = new();
    private MapBuild _nowBuild;

    public event Action<BuildData> BuildAdded;
    public event Action<MapBuild> BuildChanged;
    public event Action<bool> StateChanged;

    public void GenerateBuilds(MapManager mapManager) {
        MapData mapData = mapManager.Data;
        foreach (var buildData in mapData.BuildDatas) {
            MapBuild build = _buildPool.GetObject();
            build.SetMapManager(mapManager);
            build.ChangeState(false);
            build.Setup(mapData, buildData);
            _builds.Add(build);
            BuildAdded?.Invoke(buildData);
        }

        UpdateBuild(_builds[0]);
    }

    public void ChangeBuild(int id) {
        foreach (var build in _builds) {
            if (build.Id == id) {
                UpdateBuild(build);
                break;
            }
        }
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
        if (newState)
            UpdateBuild(_builds[0]);
        StateChanged?.Invoke(newState);
    }

    private void UpdateBuild(MapBuild build) {
        if (_nowBuild != null)
            _nowBuild.ChangeState(false);
        _nowBuild = build;
        _nowBuild.ChangeState(true);

        UpdateCameraSize();
        BuildChanged?.Invoke(build);
    }

    private void UpdateCameraSize() {
        float newSize = _nowBuild.CameraSize;
        Camera.main.orthographicSize = newSize + newSize / _cameraSizeOffset;
    }
}
