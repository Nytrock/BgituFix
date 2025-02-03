using UnityEngine;

public class MapAudienceManager : MapElementManager<MapAudience, EditableComputer, ComputerData, AudienceData> {

    [SerializeField] protected MapAudiencePool _audiencePool;
    [SerializeField] private MapEditManager _editManager;

    private void Awake() {
        _editManager.EditStateChanged += UpdateAudiences;
    }

    private void UpdateAudiences(bool isEditMode) {
        if (isEditMode)
            return;

        foreach (var audience in _mapElements)
            audience.UpdateSize();
    }

    public void GenerateAudiences(MapManager mapManager) {
        MapData mapData = mapManager.Data;
        foreach (var audienceData in mapData.AudienceDatas)
            GenerateAudience(mapData, audienceData);
    }

    private void GenerateAudience(MapData mapData, AudienceData audienceData) {
        MapAudience audience = _audiencePool.GetObject();
        audience.Setup(mapData, audienceData);
        _mapElements.Add(audience);
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
