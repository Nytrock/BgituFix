using UnityEngine;

public class MapAudienceManager : MapElementManager<MapAudience, EditableComputer, ComputerData, AudienceData> {

    [SerializeField] protected MapAudiencePool _audiencePool;

    public void UpdateAudiences() {
        foreach (var audience in _mapElements) {
            audience.UpdateSize();
            audience.UpdateContainingComputersPositions();
        }
    }

    public void GenerateAudiences(MapData mapData) {
        foreach (var audienceData in mapData.AudienceDatas)
            GenerateAudience(mapData, audienceData);
    }

    public void GenerateAudience(MapData mapData, AudienceData audienceData) {
        MapAudience audience = _audiencePool.GetObject();
        audience.Setup(mapData, audienceData);
        _mapElements.Add(audience);
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

    public void DeleteAudience(AudienceData data) {
        foreach (var audience in _mapElements) {
            if (audience.Id == data.Id) {
                audience.Delete();
                _audiencePool.PutObject(audience);
                break;
            }
        }
    }

    public void UpdateAudienceById(AudienceData data) {
        foreach (var audience in _mapElements) {
            if (audience.Id == data.Id) {
                audience.UpdateData(data);
                break;
            }
        }
    }
}
