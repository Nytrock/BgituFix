public class EditableAudiencePool : Pool<EditableAudience> {
    private MapEditManager _editManager;
    private MapManager _mapManager;
    private SelectManager _selectManager;

    public void SetManagers(MapManager mapManager, MapEditManager editManager, SelectManager selectManager) {
        _mapManager = mapManager;
        _editManager = editManager;
        _selectManager = selectManager;
    }

    protected override EditableAudience CreateObject() {
        EditableAudience audience = base.CreateObject();
        audience.SetManagers(_mapManager, _editManager, _selectManager);
        return audience;
    }
}
