using TMPro;
using UnityEngine;

public class ComputerUIManager : MonoBehaviour {
    [SerializeField] private GameObject _panel;
    [SerializeField] private MapAudienceManager _audienceManager;
    [SerializeField] private TextMeshProUGUI _audienceText;
    [SerializeField] private string _audienceMessage = "Аудитория: ";
    [SerializeField] private TextMeshProUGUI _serialNumberText;
    [SerializeField] private string _serialNumberMessage = "Серийный номер: ";
    [SerializeField] private ComputerErrorUIManager _errorsUI;


    private void Awake() {
        ChangeState(false);
    }

    public void ChangeState(bool newState) {
        _panel.SetActive(newState);
    }

    public void OpenComputer(ComputerData data) {
        UpdateData(data);
        ChangeState(true);
    }

    private void UpdateData(ComputerData data) {
        AudienceData audienceData = _audienceManager.GetElementDataById(data.AudienceId);
        _audienceText.text = _audienceMessage + audienceData.Name;
        _serialNumberText.text = _serialNumberMessage + data.SerialNumber;
        _errorsUI.SetupErrorsList(data);
    }
}
