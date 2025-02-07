using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LoginErrorMessageUI : MonoBehaviour {
    [SerializeField] private string _emptyFieldsError;
    [SerializeField] private string _loginError;

    private TextMeshProUGUI _text;


    private void Awake() {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }

    public void ShowLoginError() {
        ChangeState(true);
        _text.text = _loginError;
    }

    public void ShowEmptyFieldsError() {
        ChangeState(true);
        _text.text = _emptyFieldsError;
    }
}
