public class EditableAudiencePool : Pool<EditableAudience> {
    private MapEditManager _editManager;
    private MapManager _mapManager;

    public void SetManagers(MapManager mapManager, MapEditManager editManager) {
        _mapManager = mapManager;
        _editManager = editManager;
    }

    protected override EditableAudience CreateObject() {
        EditableAudience audience = base.CreateObject();
        audience.SetManagers(_mapManager, _editManager);
        return audience;
    }
}
