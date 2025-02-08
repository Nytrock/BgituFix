using UnityEngine;

public class MapAudiencePool : Pool<MapAudience> {
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private ComputerUIManager _computerUIManager;
    [SerializeField] private ErrorManager _errorManager;

    protected override MapAudience CreateObject() {
        MapAudience audience = base.CreateObject();
        audience.SetManagers(_mapManager, _computerUIManager, _errorManager);
        return audience;
    }

    public override MapAudience GetObject() {
        MapAudience audience = base.GetObject();
        audience.ChangeState(false);
        return audience;
    }
}
