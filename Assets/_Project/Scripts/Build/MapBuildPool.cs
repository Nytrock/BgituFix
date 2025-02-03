using UnityEngine;

public class MapBuildPool : Pool<MapBuild> {
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private UserManager _userManager;
    [SerializeField] private ErrorManager _errorManager;

    protected override MapBuild CreateObject() {
        MapBuild build = base.CreateObject();
        if (_userManager.ClientType == UserType.Admin)
            build.SetManagers(_mapManager, _errorManager);
        else
            build.SetMapManager(_mapManager);
        return build;
    }

    public override MapBuild GetObject() {
        MapBuild build = base.GetObject();
        build.ChangeState(false);
        return build;
    }
}
