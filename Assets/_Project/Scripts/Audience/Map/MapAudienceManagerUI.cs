using UnityEngine;

public class MapAudienceManagerUI : MonoBehaviour {
    [SerializeField] private MapAudienceManager _audienceManager;
    [SerializeField] private GameObject _panel;

    private void Awake() {
        _audienceManager.StateChanged += UpdateState;
    }

    private void UpdateState(bool newState) {
        _panel.SetActive(newState);
    }
}
