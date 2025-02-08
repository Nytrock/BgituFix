using System;
using UnityEngine;
using UnityEngine.UI;

public class AdminPanelManager : MonoBehaviour {
    [SerializeField] private GameObject _panel;
    [SerializeField] private UserManager _userManager;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private AdminPanelStatistics _stats;
    [SerializeField] private AdminPanelUsersList _usersList;
    [SerializeField] private Button _openButton;

    public event Action<bool> StateChanged;

    private void Awake() {
        _userManager.ClientSetuped += Setup;
        ChangeState(false);
    }

    private void Setup() {
        _openButton.gameObject.SetActive(_userManager.ClientType == UserType.Admin);
        if (_userManager.ClientType != UserType.Admin)
            return;

        _stats.Setup();
        _usersList.Setup(_userManager);
        _openButton.onClick.AddListener(Open);
    }

    private void ChangeState(bool newState) {
        _panel.SetActive(newState);
        StateChanged?.Invoke(newState);
    }

    public void Open() {
        ChangeState(true);
        _mapManager.ChangeState(false);
    }

    public void Close() {
        ChangeState(false);
        _mapManager.ChangeState(true);
    }

    public void CreateUser(UserData userData) {
        _usersList.AddUser(userData);
        _stats.AddUser();
        StartCoroutine(_userManager.CreateUser(userData));
    }
}
