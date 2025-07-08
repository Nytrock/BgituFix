using UnityEngine;

public class MapBuildFloorButton : MonoBehaviour {
    [SerializeField] private ButtonWithText _button;
    [SerializeField] private ComputerErrorRenderer _errorRenderer;
    [SerializeField] private StateStyle _selectedStyle;
    [SerializeField] private StateStyle _deselectedStyle;

    private int _floorIndex;

    public void Setup(MapBuild build, int floorIndex) {
        _floorIndex = floorIndex;
        build.FloorChanged += CheckFloor;
        CheckFloor(build.NowFloor);

        _button.SetText(floorIndex.ToString());
        _button.onClick.AddListener(delegate { build.ChangeFloor(floorIndex); });
        _errorRenderer.UpdateState();

        MapBuildFloor floor = build.GetFloor(floorIndex);
        floor.ErrorUpdated += UpdateError;
    }

    public void SetManagers(MapEditManager editManager) {
        editManager.EditStateChanged += ChangeErrorRendererState;
    }

    private void ChangeErrorRendererState(bool isEdit) {
        _errorRenderer.SetEditState(isEdit);
    }

    private void CheckFloor(int floorIndex) {
        _button.SetStyle(floorIndex == _floorIndex ? _selectedStyle : _deselectedStyle);
    }

    private void UpdateError(ComputerErrorType type) {
        _errorRenderer.SetType(type);
    }
}
