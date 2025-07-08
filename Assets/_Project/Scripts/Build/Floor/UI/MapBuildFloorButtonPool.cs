using UnityEngine;

[RequireComponent(typeof(CameraHoverTrigger))]
public class MapBuildFloorButtonPool : Pool<MapBuildFloorButton> {
    private MapEditManager _editManager;

    protected override MapBuildFloorButton CreateObject() {
        MapBuildFloorButton button = base.CreateObject();
        button.SetManagers(_editManager);
        return button;
    }

    public void SetManagers(CameraManager cameraManager, MapEditManager editManager) {
        GetComponent<CameraHoverTrigger>().SetManager(cameraManager);
        _editManager = editManager;
    }
}
