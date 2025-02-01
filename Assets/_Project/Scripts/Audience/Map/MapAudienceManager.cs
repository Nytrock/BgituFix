public class MapAudienceManager : MapElementManager<MapAudience, EditableComputer, ComputerData, AudienceData> {
    public void GenerateAudiences(MapData mapData) {
        foreach (var audienceData in mapData.AudienceDatas) {
            MapAudience audience = _mapElementsPool.GetObject();
            audience.ChangeState(false);
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
