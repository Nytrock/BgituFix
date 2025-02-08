public class EditableAudiencePool : Pool<EditableAudience> {
    private MapManager _mapManager;

    public void SetManagers(MapManager mapManager) {
        _mapManager = mapManager;
    }

    protected override EditableAudience CreateObject() {
        EditableAudience audience = base.CreateObject();
        audience.SetManagers(_mapManager);
        return audience;
    }
}
