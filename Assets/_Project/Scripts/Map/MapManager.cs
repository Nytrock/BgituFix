using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MapManager : StateMachine {
    [SerializeField] private UserManager _userManager;
    [SerializeField] private MapBuildManager _buildManager;
    [SerializeField] private MapAudienceManager _audienceManager;
    [SerializeField] private ErrorManager _errorManager;
    [SerializeField] private string _APIPathToGetBuilds;
    [SerializeField] private string _APIPathToGetAudiences;
    [SerializeField] private string _APIPathToGetComputers;

    [SerializeField] private MapData _data;

    public MapData Data => _data;

    public event Action MapGenerated;
    public event Action MapLocationChanged;

    private void Awake() {
        _userManager.ClientSetuped += StartGettingMap;
    }

    private void StartGettingMap() {
        StartCoroutine(GetMapData());
    }

    private IEnumerator GetMapData() {
        UnityWebRequest request = APIUtility.Get(_APIPathToGetBuilds);
        yield return request.SendWebRequestSafely();
        IEnumerable<BuildData> builds = request.ToData<ListData<BuildData>>().Response;

        request = APIUtility.Get(_APIPathToGetAudiences);
        yield return request.SendWebRequestSafely();
        IEnumerable<AudienceData> audiences = request.ToData<ListData<AudienceData>>().Response;

        request = APIUtility.Get(_APIPathToGetComputers);
        yield return request.SendWebRequestSafely();
        IEnumerable<ComputerData> computers = request.ToData<ListData<ComputerData>>().Response;

        _data = new(builds, audiences, computers);
        GenerateMap();
    }

    private void GenerateMap() {
        _buildManager.GenerateMapElements(_data);
        _audienceManager.GenerateMapElements(_data);
        MapGenerated?.Invoke();

        StartCoroutine(_errorManager.GetErrors(_data));
        OpenBuilds();
    }

    public void OpenBuilds() {
        _buildManager.ChangeState(true);
        _audienceManager.ChangeState(false);
        MapLocationChanged?.Invoke();
    }

    public void OpenAudience(AudienceData audience) {
        _buildManager.ChangeState(false);
        _audienceManager.OpenAudience(audience);
        MapLocationChanged?.Invoke();
    }
}
