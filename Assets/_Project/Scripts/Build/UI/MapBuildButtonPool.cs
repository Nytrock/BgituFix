using UnityEngine;

public class MapBuildButtonPool : Pool<MapBuildButton> {
    [SerializeField] private MapBuildManager _buildManager;

    protected override MapBuildButton CreateObject() {
        MapBuildButton button = base.CreateObject();
        button.SetupManagers(_buildManager);
        return button;
    }
}
