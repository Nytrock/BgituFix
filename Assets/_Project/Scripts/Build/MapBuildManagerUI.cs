using System.Collections.Generic;
using UnityEngine;

public class MapBuildManagerUI : MonoBehaviour {
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private MapBuildManager _buildManager;
    [SerializeField] private GameObject _panel;
    [SerializeField] private ButtonWithTextPool _buildsButtonsPool;
    [SerializeField] private MapBuildFloorUIPool _floorUIPool;

    private readonly List<MapBuildFloorUI> _floorsUI = new();
    private MapBuildFloorUI _nowFloor;

    private void Awake() {
        _buildManager.BuildAdded += AddBuild;
        _buildManager.BuildChanged += UpdateFloor;
        _buildManager.StateChanged += ChangeState;
    }

    private void UpdateFloor(MapBuild build) {
        if (_nowFloor != null)
            _nowFloor.ChangeState(false);

        foreach (var floorUI in _floorsUI) {
            if (floorUI.Id == build.Id) {
                _nowFloor = floorUI;
                break;
            }
        }

        _nowFloor.ChangeState(true);
    }

    private void AddBuild(MapBuild build) {
        ButtonWithText buildButton = _buildsButtonsPool.GetObject();
        buildButton.SetText(build.Name);
        buildButton.onClick.AddListener(delegate {
            _buildManager.ChangeBuild(build.Id);
        });

        MapBuildFloorUI floorUI = _floorUIPool.GetObject();
        floorUI.GenerateButtons(build);
        _floorsUI.Add(floorUI);
    }

    private void ChangeState(bool newState) {
        _panel.SetActive(newState);
    }
}
