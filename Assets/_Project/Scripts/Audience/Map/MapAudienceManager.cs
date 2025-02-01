using UnityEngine;

public class MapAudienceManager : MapElementManager<MapAudience, EditableComputer, ComputerData, AudienceData> {

    [SerializeField] protected MapAudiencePool _audiencePool;
    [SerializeField] protected ComputerUIManager _computerUIManager;

    public void GenerateAudiences(MapManager mapManager) {
        MapData mapData = mapManager.Data;
        _audiencePool.SetManagers(mapManager, _computerUIManager);

        foreach (var audienceData in mapData.AudienceDatas) {
            MapAudience audience = _audiencePool.GetObject();
            audience.Setup(mapData, audienceData);
            _mapElements.Add(audience);
        }
    }

    public void OpenAudience(AudienceData audience) {
        foreach (var mapAudience in _mapElements) {
            if (mapAudience.Id == audience.Id) {
                UpdateElement(mapAudience);
                break;
            }
        }
        ChangeState(true);
    }
}
