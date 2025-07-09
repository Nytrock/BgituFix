using UnityEngine;

public class MapBuildFloorUIPool : Pool<MapBuildFloorUI> {
    [SerializeField] private CameraManager _cameraManager;

    protected override MapBuildFloorUI CreateObject() {
        MapBuildFloorUI button = base.CreateObject();
        button.SetManagers(_cameraManager);
        return button;
    }
}
