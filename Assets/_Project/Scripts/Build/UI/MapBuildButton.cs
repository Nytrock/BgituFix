using UnityEngine;

public class MapBuildButton : MonoBehaviour {
    [SerializeField] private ButtonWithText _button;
    [SerializeField] private StateStyle _selectedStyle;
    [SerializeField] private StateStyle _deselectedStyle;

    private MapBuild _build;

    public void Setup(MapBuild build, MapBuildManager buildManager) {
        _build = build;
        _button.SetText(build.Name);
        _button.onClick.AddListener(delegate {
            buildManager.ChangeBuild(build.Id);
        });
        buildManager.BuildChanged += UpdateStyle;
    }

    private void UpdateStyle(MapBuild build) {
        _button.SetStyle(build == _build ? _selectedStyle : _deselectedStyle);
    }
}
