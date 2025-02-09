using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MapManager : MonoBehaviour {
    [SerializeField] private LoginManager _loginManager;
    [SerializeField] private UserManager _userManager;
    [SerializeField] private MapBuildManager _buildManager;
    [SerializeField] private MapAudienceManager _audienceManager;
    [SerializeField] private ErrorManager _errorManager;
    [SerializeField] private string _APIPathToGetBuilds;
    [SerializeField] private string _APIPathToGetAudiences;
    [SerializeField] private string _APIPathToGetComputers;

    [SerializeField] private MapData _data;

    public MapData Data => _data;

    public event Action<bool> StateChanged;
    public event Action MapBlocked;
    public event Action MapGenerated;

    private void Awake() {
        _userManager.ClientSetuped += CheckUserType;
    }

    private void CheckUserType() {
        if (_userManager.ClientType == UserType.None)
            BlockMap();
        else
            StartCoroutine(GetMap());
    }

    private void BlockMap() {
        ChangeState(false);
        MapBlocked?.Invoke();
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
        StateChanged?.Invoke(newState);
    }

    private IEnumerator GetMap() {
        string token = _loginManager.Token;

        UnityWebRequest request = RequestUtility.APIGet(_APIPathToGetBuilds, token);
        yield return request.SendWebRequest();
        IEnumerable<BuildData> builds = request.ToData<MapData>().BuildDatas;

        request = RequestUtility.APIGet(_APIPathToGetAudiences, token);
        yield return request.SendWebRequest();
        IEnumerable<AudienceData> audiences = request.ToData<MapData>().AudienceDatas;

        request = RequestUtility.APIGet(_APIPathToGetComputers, token);
        yield return request.SendWebRequest();
        IEnumerable<ComputerData> computers = request.ToData<MapData>().ComputerDatas;

        _data = new(builds, audiences, computers);
        GenerateMap();
    }

    private void GenerateMap() {
        _buildManager.GenerateBuilds(this);
        _audienceManager.GenerateAudiences(this);
        MapGenerated?.Invoke();

        StartCoroutine(_errorManager.GetErrors(_data));
        ChangeState(true);
        OpenBuilds();
    }

    public void OpenBuilds() {
        _buildManager.ChangeState(true);
        _audienceManager.ChangeState(false);
    }

    public void OpenAudience(AudienceData audience) {
        _buildManager.ChangeState(false);
        _audienceManager.OpenAudience(audience);
    }
}
