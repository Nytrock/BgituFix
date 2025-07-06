using UnityEngine;

public class MapBuildButtonPool : Pool<MapBuildButton> {
    [SerializeField] private MapBuildManager _buildManager;

    public override MapBuildButton GetObject() {
        MapBuildButton button = base.GetObject();
        button.SetupManagers(_buildManager);
        return button;
    }
}
