using System.Collections.Generic;
using UnityEngine;

public class MapAudience : MapElement<EditableComputer, ComputerData, AudienceData> {
    [SerializeField] private SpriteRenderer _renderer;
    private ComputerUIManager _computerUIManager;

    public override int Id => _data.Id;

    public override void Setup(MapData mapData, AudienceData data) {
        base.Setup(mapData, data);
        _renderer.size = _data.Size;
        _cameraSize = Mathf.Max(_data.Size.x / 2f / 16f * 9, _data.Size.y / 2f);
    }

    protected override IEnumerable<ComputerData> GetEditablesData(MapData mapData) {
        return mapData.GetComputersByAudience(_data);
    }

    protected override EditableComputer GenerateEditable(ComputerData data) {
        EditableComputer computer = base.GenerateEditable(data);
        computer.SetUI(_computerUIManager);
        return computer;
    }

    public void SetManagers(MapManager mapManager, ComputerUIManager computerUIManager) {
        SetMapManager(mapManager);
        _computerUIManager = computerUIManager;
    }
}
