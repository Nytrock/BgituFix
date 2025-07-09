using UnityEngine;
using UnityEngine.UI;

public class NavbarManager : MonoBehaviour {
    [SerializeField] private NavbarElement[] _elements;
    [SerializeField] private GameObject _buttonsContainer;
    [SerializeField] private LoginManager _loginManager;
    [SerializeField] private Button _logoutButton;
    [SerializeField] private UserManager _userManager;
    [SerializeField] private GameObject _adminButton;

    private NavbarElement _nowElement;

    private void Awake() {
        _loginManager.LoginStateChanged += ChangeButtonsState;
        _logoutButton.onClick.AddListener(LoginManager.Logout);

        _userManager.ClientSetuped += ChangeAdminButtonState;
        _adminButton.SetActive(false);

        SetupElements();
        OpenElement(_elements[0]);
    }

    private void ChangeAdminButtonState() {
        _adminButton.SetActive(_userManager.ClientType == UserType.Admin);
    }

    private void ChangeButtonsState(bool isLogin) {
        _buttonsContainer.SetActive(!isLogin);
        _logoutButton.gameObject.SetActive(!isLogin);
    }

    private void SetupElements() {
        foreach (var element in _elements) {
            element.Button.onClick.AddListener(delegate { OpenElement(element); });
            element.ChangeState(false);
        }
    }

    private void OpenElement(NavbarElement element) {
        if (_nowElement == element)
            return;

        if (_nowElement != null)
            _nowElement.ChangeState(false);
        _nowElement = element;
        _nowElement.ChangeState(true);
    }
}
