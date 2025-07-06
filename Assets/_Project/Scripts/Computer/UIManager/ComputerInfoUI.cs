using TMPro;
using UnityEngine;

public class ComputerInfoUI : MonoBehaviour {
    [SerializeField] private MapAudienceManager _audienceManager;
    [SerializeField] private TextMeshProUGUI _audienceText;
    [SerializeField] private string _audienceMessage = "Аудитория: ";
    [SerializeField] private TextMeshProUGUI _serialNumberText;
    [SerializeField] private ComputerErrorsUI _errorsUI;

    public void Open(ComputerData data) {
        AudienceData audienceData = _audienceManager.GetElementDataById(data.AudienceId);
        _audienceText.text = _audienceMessage + audienceData.Name;
        _serialNumberText.text = data.SerialNumber;
        _errorsUI.SetupErrorsList(data);
        ChangeState(true);
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }

    public void Close() {
        _errorsUI.Close();
    }
}
