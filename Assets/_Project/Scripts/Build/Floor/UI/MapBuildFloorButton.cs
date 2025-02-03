using UnityEngine;

[RequireComponent(typeof(ButtonWithText))]
public class MapBuildFloorButton : MonoBehaviour {
    [SerializeField] private ComputerErrorRenderer _errorRenderer;
    private ButtonWithText _button;

    private void Awake() {
        _button = GetComponent<ButtonWithText>();
    }

    public void Setup(MapBuild build, int floorIndex) {
        _button.SetText(floorIndex.ToString());
        _button.onClick.AddListener(delegate { build.ChangeFloor(floorIndex); });
        _errorRenderer.ChangeState(false);

        MapBuildFloor floor = build.GetFloor(floorIndex);
        floor.ErrorUpdated += CheckError;
    }

    private void CheckError(ComputerErrorType type) {
        _errorRenderer.ChangeState(type != ComputerErrorType.None);
        if (type != ComputerErrorType.None)
            _errorRenderer.SetType(type);
    }
}
