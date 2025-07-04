using UnityEngine;

public class MapBuildFloorUIPool : Pool<MapBuildFloorUI> {
    [SerializeField] private CameraManager _cameraManager;

    public override MapBuildFloorUI GetObject() {
        MapBuildFloorUI button = base.GetObject();
        button.SetManagers(_cameraManager);
        return button;
    }
}
