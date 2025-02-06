using UnityEngine;

public class MapBuildButton : MonoBehaviour {
    [SerializeField] private string _buildText;
    [SerializeField] private ButtonWithText _button;
    [SerializeField] private StateStyle _selectedStyle;
    [SerializeField] private StateStyle _deselectedStyle;

    private MapBuild _build;

    public void Setup(MapBuild build, MapBuildManager buildManager) {
        _build = build;
        _button.SetText(_buildText + ' ' + build.Number);
        _button.onClick.AddListener(delegate {
            buildManager.ChangeBuild(build.Id);
        });
        buildManager.BuildChanged += UpdateStyle;
    }

    private void UpdateStyle(MapBuild build) {
        _button.SetStyle(build == _build ? _selectedStyle : _deselectedStyle);
    }
}
