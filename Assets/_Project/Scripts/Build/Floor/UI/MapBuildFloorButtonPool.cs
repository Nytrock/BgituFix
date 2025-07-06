using UnityEngine;

[RequireComponent(typeof(CameraHoverTrigger))]
public class MapBuildFloorButtonPool : Pool<MapBuildFloorButton> {
    public void SetManagers(CameraManager cameraManager) {
        GetComponent<CameraHoverTrigger>().SetManager(cameraManager);
    }
}
