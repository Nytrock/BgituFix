using UnityEngine;

public class MapManagerUI : MonoBehaviour {
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _blockMessage;

    private void Awake() {
        _mapManager.BlockStateChanged += ChangeBlockState;
    }

    private void ChangeBlockState(bool isBlocked) {
        _panel.SetActive(!isBlocked);
        _blockMessage.SetActive(isBlocked);
    }
}
