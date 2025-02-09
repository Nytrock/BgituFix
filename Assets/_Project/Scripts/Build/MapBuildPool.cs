using UnityEngine;

public class MapBuildPool : Pool<MapBuild> {
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private UserManager _userManager;
    [SerializeField] private ErrorManager _errorManager;
    [SerializeField] private CameraManager _cameraManager;

    protected override MapBuild CreateObject() {
        MapBuild build = base.CreateObject();
        build.SetManagers(_mapManager, _errorManager, _userManager);
        return build;
    }

    public override MapBuild GetObject() {
        MapBuild build = base.GetObject();
        build.ChangeState(false);
        return build;
    }
}
