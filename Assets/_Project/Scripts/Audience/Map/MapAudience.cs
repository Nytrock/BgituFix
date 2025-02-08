using System.Collections.Generic;
using UnityEngine;

public class MapAudience : MapElement<EditableComputer, ComputerData, AudienceData> {
    [SerializeField] private SpriteRenderer _renderer;

    public override int Id => _data.Id;

    public override void Setup(MapData mapData, AudienceData data) {
        base.Setup(mapData, data);
        UpdateSize();
    }

    public void UpdateSize() {
        _renderer.size = _data.SizeVector;
        _cameraSize = Mathf.Max(_data.SizeVector.x / 2f / 16f * 9, _data.SizeVector.y / 2f);
    }

    protected override IEnumerable<ComputerData> GetEditablesData(MapData mapData) {
        return mapData.GetComputersByAudience(_data);
    }

    public void SetManagers(MapManager mapManager, ComputerUIManager computerUI, ErrorManager errorManager) {

        (_pool as EditableComputerPool).SetManagers(mapManager, computerUI);

        errorManager.ErrorAdded += CheckNewError;
        errorManager.ErrorChanged += CheckChangedError;
        errorManager.ErrorDeleted += CheckDeletedError;
    }

    public void CheckNewError(ComputerErrorData errorData) {
        foreach (var computer in _editables)
            computer.CheckNewError(errorData);
    }

    public void CheckChangedError(ComputerErrorData errorData) {
        foreach (var computer in _editables)
            computer.CheckChangedError(errorData);
    }

    public void CheckDeletedError(ComputerErrorData errorData) {
        foreach (var computer in _editables)
            computer.CheckDeletedError(errorData);
    }

    public void Delete() {
        foreach (var computer in _editables)
            _pool.PutObject(computer);
        _editables.Clear();
    }
}
