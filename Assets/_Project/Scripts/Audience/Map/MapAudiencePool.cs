public class MapAudiencePool : Pool<MapAudience> {
    private MapManager _mapManager;
    private ComputerUIManager _computerUIManager;

    public void SetManagers(MapManager mapManager, ComputerUIManager computerUIManager) {
        _mapManager = mapManager;
        _computerUIManager = computerUIManager;
    }

    protected override MapAudience CreateObject() {
        MapAudience audience = base.CreateObject();
        audience.SetManagers(_mapManager, _computerUIManager);
        return audience;
    }

    public override MapAudience GetObject() {
        MapAudience audience = base.GetObject();
        audience.ChangeState(false);
        return audience;
    }
}
