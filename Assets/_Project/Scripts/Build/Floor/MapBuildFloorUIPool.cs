using UnityEngine;

public class MapBuildFloorUIPool : Pool<MapBuildFloorUI> {
    [SerializeField] private CameraManager _cameraManager;

    protected override MapBuildFloorUI CreateObject() {
        MapBuildFloorUI floorUI = base.CreateObject();
        floorUI.SetCameraManager(_cameraManager);
        return floorUI;
    }
}
