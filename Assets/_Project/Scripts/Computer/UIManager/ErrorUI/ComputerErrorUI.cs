using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputerErrorUI : MonoBehaviour {
    [SerializeField] private ComputerErrorRenderer _errorRenderer;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Toggle _solveToggle;
    [SerializeField] private Button _deleteButton;

    private ComputerErrorsUI _errorManager;
    private ComputerErrorData _data;

    public ComputerErrorType ErrorType => _data.Type;
    public bool IsSolved => _data.IsSolved;

    public void SetError(ComputerErrorData error, UserManager userManager) {
        _data = error;
        _errorRenderer.SetType(error.Type);
        _descriptionText.text = error.Comment;
        _nameText.text = userManager.GetUserDataById(error.UserId).Name;
        _solveToggle.SetIsOnWithoutNotify(error.IsSolved);

        bool isAdmin = userManager.ClientType == UserType.Admin;
        _solveToggle.gameObject.SetActive(isAdmin);
        _deleteButton.gameObject.SetActive(isAdmin || userManager.ClientId == error.UserId);
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }

    public void SetErrorManager(ComputerErrorsUI errorManager) {
        _errorManager = errorManager;
        _solveToggle.onValueChanged.AddListener(ChangeSolved);
        _deleteButton.onClick.AddListener(Delete);
    }

    private void Delete() {
        _errorManager.DeleteError(_data, this);
    }

    private void ChangeSolved(bool isSolved) {
        _errorManager.ChangeErrorSolve(_data, isSolved);
    }
}
