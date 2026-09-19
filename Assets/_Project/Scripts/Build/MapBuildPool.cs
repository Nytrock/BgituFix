using UnityEngine;

public class MapBuildPool : ResolverPool<MapBuild> {
    public override MapBuild GetObject() {
        MapBuild build = base.GetObject();
        build.ChangeState(false);
        return build;
    }
}
