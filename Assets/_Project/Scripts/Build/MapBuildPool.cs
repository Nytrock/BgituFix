public class MapBuildPool : Pool<MapBuild> {
    private MapManager _mapManager;

    public void SetManagers(MapManager mapManager) {
        _mapManager = mapManager;
    }

    protected override MapBuild CreateObject() {
        MapBuild build = base.CreateObject();
        build.SetMapManager(_mapManager);
        return build;
    }

    public override MapBuild GetObject() {
        MapBuild build = base.GetObject();
        build.ChangeState(false);
        return build;
    }
}
