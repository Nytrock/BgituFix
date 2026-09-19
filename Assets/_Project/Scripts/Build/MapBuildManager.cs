using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuildManager : MapElementManager<MapBuild> {
    [SerializeField] private MapAudienceManager _audienceManager;
    [SerializeField] private ErrorManager _errorManager;

    public event Action<MapBuild> BuildAdded;
    public event Action<MapBuild> BuildChanged;

    public override void GenerateMapElements(MapData mapData) {
        base.GenerateMapElements(mapData);
        SelectElement(_mapElements[0]);
    }

    public override void GenerateMapElement(IdData elementData) {
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

    public override IEnumerator DeleteEditableAfterEditing(EditableData audienceData) {
        foreach (var computerData in _mapData.GetComputersByAudienceId(audienceData.Id))
            _errorManager.DeleteErrorsByComputerId(computerData.Id);
        _audienceManager.DeleteAudienceById(audienceData.Id);
        return base.DeleteEditableAfterEditing(audienceData);
    }

    public override IEnumerator CreateEditableAfterEditing(EditableData editableData) {
        _audienceManager.GenerateMapElement(editableData);
        return base.CreateEditableAfterEditing(editableData);
    }

    public override void ChangeEditableOnMap(EditableData editableData) {
        _audienceManager.UpdateAudienceById(editableData as AudienceData);
        base.ChangeEditableOnMap(editableData);
    }

    protected override EditableData GetEditableById(int id) {
        return _mapData.GetAudienceById(id);
    }

    protected override IEnumerable<IdData> GetElementsData() {
        return _mapData.BuildDatas;
    }

    protected override IEnumerable<EditableData> GetEditablesData(MapData mapData) {
        return mapData.AudienceDatas;
    }
}
