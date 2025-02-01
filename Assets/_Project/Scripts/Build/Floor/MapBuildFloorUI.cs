using UnityEngine;

public class MapBuildFloorUI : MonoBehaviour {
    [SerializeField] private ButtonWithTextPool _buttonsPool;
    private MapBuild _build;

    public int Id => _build.Id;

    public void GenerateButtons(MapBuild build) {
        for (int i = 1; i <= build.FloorsCount; i++) {
            ButtonWithText button = _buttonsPool.GetObject();
            button.SetText(i.ToString());

            int index = i;
            button.onClick.AddListener(delegate { build.ChangeFloor(index); });
        }

        _build = build;
        ChangeState(false);
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }
}
