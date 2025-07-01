using UnityEngine;

public class MapManagerUI : MonoBehaviour {
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private GameObject _panel;

    private void Awake() {
        _mapManager.StateChanged += ChangeState;
    }

    private void ChangeState(bool newState) {
        _panel.SetActive(newState);
    }
}
