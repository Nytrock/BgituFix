using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputerErrorFormUI : MonoBehaviour {
    [SerializeField] private ErrorManager _errorManager;
    [SerializeField] private TMP_Dropdown _errorType;
    [SerializeField] private TMP_InputField _errorDescription;
    [SerializeField] private Button _closeButton;
    [SerializeField] private StateMachine _helpPanel;

    private ComputerData _computerData;

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
        _helpPanel.ChangeState(false);
        if (!newState)
            ClearForm();
    }

    private void ClearForm() {
        _errorType.value = 0;
        _errorDescription.text = "";
    }

    public void CreateError() {
        if (string.IsNullOrEmpty(_errorDescription.text))
            return;

        ComputerErrorType type = (ComputerErrorType)(_errorType.value + 1);
        string comment = _errorDescription.text;
        StartCoroutine(_errorManager.CreateErrorInDatabase(type, comment, _computerData.Id));
        _closeButton.onClick.Invoke();
    }

    public void SetComputer(ComputerData data) {
        _computerData = data;
    }

    public void Close() {
        _computerData = null;
    }

    public void ChangeHelpState(bool newState) {
        _helpPanel.ChangeState(newState);
        gameObject.SetActive(!newState);
    }
}
