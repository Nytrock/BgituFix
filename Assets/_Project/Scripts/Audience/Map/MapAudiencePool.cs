using UnityEngine;

public class MapAudiencePool : ResolverPool<MapAudience> {
    public override MapAudience GetObject() {
        MapAudience audience = base.GetObject();
        audience.ChangeState(false);
        return audience;
    }
}
