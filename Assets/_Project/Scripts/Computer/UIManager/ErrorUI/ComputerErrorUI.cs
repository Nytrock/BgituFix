using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputerErrorUI : MonoBehaviour {
    [SerializeField] private ComputerErrorRenderer _errorRenderer;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _dateText;
    [SerializeField] private ComputerErrorSolveButton _solveButton;
    [SerializeField] private Button _deleteButton;

    private ComputerErrorsUI _errorManager;
    private ComputerErrorData _data;

    public ComputerErrorType ErrorType => _data.Type;
    public bool IsSolved => _data.IsSolved;
    public int Id => _data.Id;

    public void SetError(ComputerErrorData error, UserManager userManager) {
        _data = error;
        _errorRenderer.SetType(error.Type);
        _descriptionText.text = error.Comment;
        _dateText.text = error.Date;
        _nameText.text = userManager.GetUserDataById(error.UserId).Name;

        bool isAdmin = userManager.ClientType == UserType.Admin;
        _deleteButton.gameObject.SetActive(isAdmin || userManager.ClientId == error.UserId);
        _solveButton.Setup(error, userManager.ClientType);
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }

    public void SetErrorManager(ComputerErrorsUI errorManager) {
        _errorManager = errorManager;
        _solveButton.SetupButton();
        _solveButton.OnValueChanged += ChangeSolved;
        _deleteButton.onClick.AddListener(Delete);
    }

    private void Delete() {
        _errorManager.DeleteError(_data);
    }

    private void ChangeSolved(bool isSolved) {
        _errorManager.ChangeErrorSolve(_data, isSolved);
    }

    public void ClearData() {
        _data = null;
    }

    public void UpdateSolved() {
        _solveButton.SetIsSolvedWithoutNotify(_data.IsSolved);
    }
}
