using System;
using System.Collections.Generic;
using UnityEngine;

public class MapBuildManager : MapElementManager<MapBuild, EditableAudience, AudienceData, BuildData> {
    [SerializeField] private MapAudienceManager _audienceManager;
    [SerializeField] private ErrorManager _errorManager;

    public event Action<MapBuild> BuildAdded;
    public event Action<MapBuild> BuildChanged;

    public override void GenerateMapElements(MapData mapData) {
        base.GenerateMapElements(mapData);
        SelectElement(_mapElements[0]);
    }

    public override void GenerateMapElement(BuildData elementData) {
        base.GenerateMapElement(elementData);
        BuildAdded?.Invoke(_mapElements[^1]);
    }

    public void OpenBuild(int id) {
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

    protected override AudienceData GetEditableById(int id) {
        return _mapData.GetAudienceById(id);
    }

    protected override IEnumerable<BuildData> GetElementsData() {
        return _mapData.BuildDatas;
    }

    protected override IEnumerable<AudienceData> GetEditablesData(MapData mapData) {
        return mapData.AudienceDatas;
    }
}
