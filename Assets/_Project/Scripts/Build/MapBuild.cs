using System.Collections.Generic;
using UnityEngine;

public class MapBuild : MapElement<EditableAudience, AudienceData, BuildData> {
    [SerializeField] private Transform[] _floors;

    protected override void GenerateEditables(MapData mapData) {
        _floors = new Transform[_data.FloorsCount];
        for (int i = 0; i < _data.FloorsCount; i++) {
            GameObject floor = new($"Floor {i + 1}");
            _floors[i] = floor.transform;
            _floors[i].parent = transform;
        }
        base.GenerateEditables(mapData);
    }

    protected override EditableAudience GenerateEditable(AudienceData data) {
        EditableAudience audience = base.GenerateEditable(data);
        audience.transform.parent = _floors[data.Floor - 1];
        return audience;
    }

    protected override IEnumerable<AudienceData> GetEditablesData(MapData mapData) {
        return mapData.GetAudiencesByBuild(_data);
    }
}
