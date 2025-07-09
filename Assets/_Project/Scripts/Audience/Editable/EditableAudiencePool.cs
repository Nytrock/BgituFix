public class EditableAudiencePool : Pool<EditableAudience> {
    private MapManager _mapManager;
    private SelectManager _selectManager;

    public void SetManagers(MapManager mapManager, SelectManager selectManager) {
        _mapManager = mapManager;
        _selectManager = selectManager;
    }

    protected override EditableAudience CreateObject() {
        EditableAudience audience = base.CreateObject();
        audience.SetManagers(_mapManager, _selectManager);
        return audience;
    }
}
