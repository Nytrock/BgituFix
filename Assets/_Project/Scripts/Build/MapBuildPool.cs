using UnityEngine;

public class MapBuildPool : Pool<MapBuild> {
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private MapEditManager _editManager;
    [SerializeField] private UserManager _userManager;
    [SerializeField] private ErrorManager _errorManager;

    protected override MapBuild CreateObject() {
        MapBuild build = base.CreateObject();
        build.SetManagers(_mapManager, _editManager, _errorManager, _userManager);
        return build;
    }

    public override MapBuild GetObject() {
        MapBuild build = base.GetObject();
        build.ChangeState(false);
        return build;
    }
}
