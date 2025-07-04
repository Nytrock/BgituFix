public class MapBuildFloorButtonPool : Pool<MapBuildFloorButton> {
    private CameraManager _cameraManager;

    public override MapBuildFloorButton GetObject() {
        MapBuildFloorButton button = base.GetObject();
        button.SetManagers(_cameraManager);
        return button;
    }

    public void SetManagers(CameraManager cameraManager) {
        _cameraManager = cameraManager;
    }
}
