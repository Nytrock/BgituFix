using UnityEngine;

public class MapBuildFloorUI : MonoBehaviour {
    [SerializeField] private MapBuildFloorButtonPool _buttonsPool;
    private MapBuild _build;

    public int Id => _build.Id;

    public void GenerateButtons(MapBuild build, MapManager mapManager) {
        for (int i = 1; i <= build.FloorsCount; i++) {
            MapBuildFloorButton button = _buttonsPool.GetObject();
            button.Setup(build, i, mapManager);
        }

        _build = build;
        ChangeState(false);
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }
}
