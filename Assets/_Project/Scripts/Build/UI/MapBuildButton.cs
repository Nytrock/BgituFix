using UnityEngine;

public class MapBuildButton : MonoBehaviour {
    [SerializeField] private ButtonWithText _button;
    [SerializeField] private StateStyle _selectedStyle;
    [SerializeField] private StateStyle _deselectedStyle;

    private MapBuildManager _buildManager;
    private MapBuild _build;

    public void SetBuild(MapBuild build) {
        _build = build;
        _button.onClick.AddListener(delegate {
            _buildManager.OpenBuild(build.Id);
        });
        _button.SetText(build.Number.ToString());
    }

    public void SetupManagers(MapBuildManager buildManager) {
        _buildManager = buildManager;
        buildManager.BuildChanged += UpdateStyle;
    }

    private void UpdateStyle(MapBuild build) {
        _button.SetStyle(build == _build ? _selectedStyle : _deselectedStyle);
    }
}
