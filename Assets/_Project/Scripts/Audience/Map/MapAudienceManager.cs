using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAudienceManager : MapElementManager<MapAudience> {
    [SerializeField] private ErrorManager _errorManager;

    public void UpdateAudiences() {
        foreach (var audience in _mapElements) {
            audience.UpdateSize();
            audience.UpdateContainingComputersPositions();
        }
    }

    public void OpenAudience(AudienceData audience) {
        foreach (var mapAudience in _mapElements) {
            if (mapAudience.Id == audience.Id) {
                SelectElement(mapAudience);
                break;
            }
        }
        ChangeState(true);
    }

    public void DeleteAudienceById(int id) {
        foreach (var audience in _mapElements) {
            if (audience.Id == id) {
                audience.Delete();
                _pool.PutObject(audience);
                break;
            }
        }
    }

    public override IEnumerator DeleteEditableAfterEditing(EditableData editableData) {
        _errorManager.DeleteErrorsByComputerId(editableData.Id);
        return base.DeleteEditableAfterEditing(editableData);
    }

    public void UpdateAudienceById(AudienceData data) {
        foreach (var audience in _mapElements) {
            if (audience.Id == data.Id) {
                audience.UpdateData(data);
                break;
            }
        }
    }

    protected override EditableData GetEditableById(int id) {
        return _mapData.GetComputerById(id);
    }

    protected override IEnumerable<IdData> GetElementsData() {
        return _mapData.AudienceDatas;
    }

    protected override IEnumerable<EditableData> GetEditablesData(MapData mapData) {
        return mapData.ComputerDatas;
    }
}
