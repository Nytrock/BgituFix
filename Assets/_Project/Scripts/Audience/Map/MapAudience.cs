using System.Collections.Generic;
using UnityEngine;

public class MapAudience : MapElement<EditableComputer, ComputerData, AudienceData> {
    [SerializeField] private SpriteRenderer _renderer;

    public int Id => _data.Id;
    public float CameraSize => Mathf.Max(_data.Width / 32f * 9, _data.Length / 2f);

    public override void Setup(MapData mapData, AudienceData data) {
        base.Setup(mapData, data);
        _renderer.size = new(_data.Width, _data.Length);
    }

    protected override IEnumerable<ComputerData> GetEditablesData(MapData mapData) {
        return mapData.GetComputersByAudience(_data);
    }
}
