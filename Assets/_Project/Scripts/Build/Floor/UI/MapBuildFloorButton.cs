using UnityEngine;

[RequireComponent(typeof(ButtonWithText))]
public class MapBuildFloorButton : MonoBehaviour {
    [SerializeField] private ComputerErrorRenderer _errorRenderer;
    private ButtonWithText _button;

    private void Awake() {
        _button = GetComponent<ButtonWithText>();
    }

    public void Setup(MapBuild build, int floorIndex, MapManager mapManager) {
        _button.SetText(floorIndex.ToString());
        _button.onClick.AddListener(delegate { build.ChangeFloor(floorIndex); });

        ComputerErrorType type = mapManager.Data.GetFloorMaxErrorType(build.Id, floorIndex);
        _errorRenderer.ChangeState(type != ComputerErrorType.None);
        _errorRenderer.SetVisual(type);
    }
}
