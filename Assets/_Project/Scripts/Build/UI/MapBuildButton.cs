using UnityEngine;

[RequireComponent(typeof(CameraHoverTrigger))]
public class MapBuildButton : MonoBehaviour {
    [SerializeField] private string _buildText;
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
        _button.SetText(_buildText + ' ' + build.Number);
    }

    public void SetupManagers(MapBuildManager buildManager, CameraManager cameraManager) {
        GetComponent<CameraHoverTrigger>().SetManager(cameraManager);

        _buildManager = buildManager;
        buildManager.BuildChanged += UpdateStyle;
    }

    private void UpdateStyle(MapBuild build) {
        _button.SetStyle(build == _build ? _selectedStyle : _deselectedStyle);
    }
}
