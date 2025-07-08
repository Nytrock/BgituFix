using UnityEngine;

public class MapBuildFloorUIPool : Pool<MapBuildFloorUI> {
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private MapEditManager _editManager;

    protected override MapBuildFloorUI CreateObject() {
        MapBuildFloorUI button = base.CreateObject();
        button.SetManagers(_cameraManager, _editManager);
        return button;
    }
}
