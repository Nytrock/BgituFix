using TMPro;
using UnityEngine;

public class ProfileErrorRenderer : MonoBehaviour {
    [SerializeField] private ComputerErrorRenderer _errorRenderer;
    [SerializeField] private TextMeshProUGUI _commentText;
    [SerializeField] private TextMeshProUGUI _dateText;
    [SerializeField] private ComputerErrorSolveButton _solveButton;

    private ErrorManager _errorManager;
    private ComputerErrorData _errorData;

    public int Id => _errorData.Id;

    public void SetError(ComputerErrorData error, ErrorManager errorManager) {
        _errorData = error;
        _errorManager = errorManager;

        _errorRenderer.SetType(error.Type);
        _commentText.text = error.Comment;
        _dateText.text = error.Date;

        _solveButton.Setup(error);
        _solveButton.OnValueChanged += ChangeErrorSolved;
    }

    public void SetIsSolved(bool isSolved) {
        _solveButton.SetIsSolvedWithoutNotify(isSolved);
    }

    private void ChangeErrorSolved(bool isSolved) {
        StartCoroutine(_errorManager.ChangeErrorSolveInDatabase(_errorData, isSolved));
    }

    public void DeleteError() {
        StartCoroutine(_errorManager.DeleteErrorInDatabase(_errorData));
    }
}
