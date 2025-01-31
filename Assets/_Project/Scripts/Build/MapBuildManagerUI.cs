using System.Collections.Generic;
using UnityEngine;

public class MapBuildManagerUI : MonoBehaviour {
    [SerializeField] private MapBuildManager _manager;
    [SerializeField] private GameObject _panel;
    [SerializeField] private ButtonWithTextPool _buildsButtonsPool;
    [SerializeField] private ButtonWithTextPool _floorButtonsPool;

    private readonly List<ButtonWithText> _floorButtons = new();

    private void Awake() {
        _manager.BuildAdded += AddBuild;
        _manager.BuildChanged += UpdateFloorButtons;
        _manager.StateChanged += ChangeState;
    }

    private void UpdateFloorButtons(MapBuild build) {
        foreach (var button in _floorButtons)
            _floorButtonsPool.PutObject(button);
        _floorButtons.Clear();

        for (int i = 1; i <= build.FloorsCount; i++) {
            ButtonWithText button = _floorButtonsPool.GetObject();
            button.SetText(i.ToString());

            int index = i;
            button.onClick.AddListener(delegate { build.ChangeFloor(index); });
            _floorButtons.Add(button);
        }
    }

    private void AddBuild(BuildData data) {
        ButtonWithText buildButton = _buildsButtonsPool.GetObject();
        buildButton.SetText(data.Name);
        buildButton.onClick.AddListener(delegate {
            _manager.ChangeBuild(data.Id);
        });
    }

    private void ChangeState(bool newState) {
        _panel.SetActive(newState);
    }
}
