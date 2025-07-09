using System.Collections.Generic;
using UnityEngine;

public class MapAudienceManager : MapElementManager<MapAudience, EditableComputer, ComputerData, AudienceData> {
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

    protected override ComputerData GetEditableById(int id) {
        return _mapData.GetComputerById(id);
    }

    protected override IEnumerable<AudienceData> GetElementsData() {
        return _mapData.AudienceDatas;
    }

    protected override IEnumerable<ComputerData> GetEditablesData(MapData mapData) {
        return mapData.ComputerDatas;
    }
}
