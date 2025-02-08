using System.Linq;
using TMPro;
using UnityEngine;

public class AdminPanelStatistics : MonoBehaviour {
    [SerializeField] private UserManager _usersManager;
    [SerializeField] private TextMeshProUGUI _usersCountText;
    [SerializeField] private ErrorManager _errorsManager;
    [SerializeField] private TextMeshProUGUI _errorsCountText;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private TextMeshProUGUI _computersCountText;

    private int _usersCount;

    public void Setup() {
        _usersManager.UsersGetted += UpdateUsers;
        _usersManager.UsersCountChanged += UpdateUsers;

        _mapManager.MapGenerated += UpdateMap;

        _errorsManager.ErrorsLoaded += UpdateErrors;
        _errorsManager.ErrorsUpdated += UpdateErrors;
    }

    private void UpdateUsers() {
        _usersCount = _usersManager.UsersCount;
        _usersCountText.text = _usersCount.ToString();
    }

    private void UpdateMap() {
        int computersCount = _mapManager.Data.ComputerDatas.Count();
        _computersCountText.text = computersCount.ToString();
    }

    private void UpdateErrors() {
        int errorsCount = _errorsManager.Errors.Count();
        _errorsCountText.text = errorsCount.ToString();
    }

    public void AddUser() {
        _usersCount++;
        _usersCountText.text = _usersCount.ToString();
    }
}
