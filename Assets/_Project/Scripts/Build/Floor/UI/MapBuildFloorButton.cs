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
        _errorRenderer.ChangeState(false);

        MapBuildFloor floor = build.GetFloor(floorIndex);
        floor.ErrorUpdated += CheckError;
    }

    private void CheckFloor(int floorIndex) {
        _button.SetStyle(floorIndex == _floorIndex ? _selectedStyle : _deselectedStyle);
    }

    private void CheckError(ComputerErrorType type) {
        _errorRenderer.ChangeState(type != ComputerErrorType.None);
        if (type != ComputerErrorType.None)
            _errorRenderer.SetType(type);
    }
}
