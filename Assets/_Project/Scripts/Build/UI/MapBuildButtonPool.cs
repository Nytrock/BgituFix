using UnityEngine;

public class MapBuildButtonPool : Pool<MapBuildButton> {
    [SerializeField] private MapBuildManager _buildManager;
    [SerializeField] private CameraManager _cameraManager;

    public override MapBuildButton GetObject() {
        MapBuildButton button = base.GetObject();
        button.SetupManagers(_buildManager, _cameraManager);
        return button;
    }
}
