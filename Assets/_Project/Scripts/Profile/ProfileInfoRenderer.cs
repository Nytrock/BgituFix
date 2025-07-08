using TMPro;
using UnityEngine;

public class ProfileInfoRenderer : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _emailText;
    [SerializeField] private TextMeshProUGUI _errorsFoundText;
    [SerializeField] private TextMeshProUGUI _errorsLeftText;

    private UserData _clientData;
    private int _errorsFound;
    private int _errorsFixed;

    public void SetInfo(UserData userData, ErrorManager errorManager) {
        _clientData = userData;
        _nameText.text = userData.Name;
        _emailText.text = userData.Email;

        errorManager.ErrorAdded += CheckNewError;
        errorManager.ErrorChanged += CheckChangedError;
        errorManager.ErrorDeleted += CheckDeletedError;
        UpdateErrorFields();
    }

    private void CheckChangedError(ComputerErrorData error) {
        if (error.UserId != _clientData.Id)
            return;

        if (error.IsSolved)
            _errorsFixed++;
        else
            _errorsFixed--;
        UpdateErrorFields();
    }

    private void CheckNewError(ComputerErrorData error) {
        if (error.UserId != _clientData.Id)
            return;

        _errorsFound++;
        if (error.IsSolved)
            _errorsFixed++;
        UpdateErrorFields();
    }

    private void CheckDeletedError(ComputerErrorData error) {
        if (error.UserId != _clientData.Id)
            return;

        _errorsFound--;
        if (error.IsSolved)
            _errorsFixed--;
        UpdateErrorFields();
    }

    private void UpdateErrorFields() {
        _errorsFoundText.text = _errorsFound.ToString();
        _errorsLeftText.text = (_errorsFound - _errorsFixed).ToString();
    }
}
