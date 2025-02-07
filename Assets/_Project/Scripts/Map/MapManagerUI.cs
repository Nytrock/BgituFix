using UnityEngine;

public class MapManagerUI : MonoBehaviour {
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _blockMessage;

    private void Awake() {
        _mapManager.StateChanged += ChangeState;
        _mapManager.MapBlocked += BlockMap;
    }

    private void BlockMap() {
        _blockMessage.SetActive(true);
    }

    private void ChangeState(bool newState) {
        _panel.SetActive(newState);
    }
}
