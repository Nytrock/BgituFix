using UnityEngine;

public class ComputerUIManager : MonoBehaviour {
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _mapPanel;
    [SerializeField] private ComputerInfoUI _infoManager;
    [SerializeField] private ComputerErrorFormUI _errorForm;

    private void Awake() {
        ChangeState(false);
    }

    public void ChangeState(bool newState) {
        _panel.SetActive(newState);
        _mapPanel.SetActive(!newState);
        if (!newState) {
            _infoManager.Close();
            _errorForm.Close();
        }
    }

    public void OpenComputer(ComputerData data) {
        _infoManager.Open(data);
        _errorForm.SetComputer(data);
        ChangeInfoState(true);
        ChangeState(true);
    }

    public void OpenForm() {
        ChangeInfoState(false);
    }

    public void ChangeInfoState(bool newState) {
        _infoManager.ChangeState(newState);
        _errorForm.ChangeState(!newState);
    }
}
