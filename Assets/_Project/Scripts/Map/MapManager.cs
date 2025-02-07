using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public EditableAudience CreateNewAudience() {
        int buildId = _buildManager.NowElement.Id;
        int floorIndex = _buildManager.NowElement.NowFloor;
        int audiencesCount = _data.AudienceDatas.Count();
        return CreateAudience(new(buildId, floorIndex, audiencesCount));
    }

    public EditableAudience CreateAudience(AudienceData audienceData) {
        _data.AddAudience(audienceData);
        return _buildManager.CreateEditable(audienceData);
    }

    public void DeleteAudience(EditableAudience audience) {
        foreach (var computerData in _data.GetComputersByAudience(audience.Data))
            _errorManager.DeleteErrorsByComputerId(computerData);

        _buildManager.DeleteEditable(audience);
        _audienceManager.DeleteAudience(audience.Data);
        _data.DeleteAudience(audience.Data);
    }

    public EditableComputer CreateNewComputer() {
        int audienceId = _audienceManager.NowElement.Id;
        int computersCount = _data.ComputerDatas.Count();
        return CreateComputer(new(audienceId, computersCount));
    }

    public EditableComputer CreateComputer(ComputerData computerData) {
        _data.AddComputer(computerData);
        return _audienceManager.CreateEditable(computerData);
    }

    public void DeleteComputer(EditableComputer computer) {
        _audienceManager.DeleteEditable(computer);
        _errorManager.DeleteErrorsByComputerId(computer.Data);
        _data.DeleteComputer(computer.Data);
    }

    public IEnumerator RevertMapData(MapData _) {
        yield break;
    }

    public IEnumerator CheckUpdatedData(MapData oldMapData) {
        foreach (var oldData in oldMapData.AudienceDatas) {
            if (!_data.ContainsAudience(oldData)) {
                yield return _buildManager.DeleteEditableInDatabase(oldData);
            } else {
                AudienceData newData = _data.GetAudienceById(oldData.Id);
                if (!newData.Equals(oldData))
                    yield return _buildManager.ChangeEditableInDatabase(newData);
            }
        }

        foreach (var newData in _data.AudienceDatas) {
            if (newData.Id == -1) {
                yield return _buildManager.CreateEditableInDatabase(newData);
                _audienceManager.GenerateAudience(_data, newData);
            }
        }

        foreach (var oldData in oldMapData.ComputerDatas) {
            if (!_data.ContainsComputer(oldData)) {
                yield return _audienceManager.DeleteEditableInDatabase(oldData);
            } else {
                ComputerData newData = _data.GetComputerById(oldData.Id);
                if (!newData.Equals(oldData))
                    yield return _audienceManager.ChangeEditableInDatabase(newData);
            }
        }

        foreach (var newData in _data.ComputerDatas)
            if (newData.Id == -1)
                yield return _audienceManager.CreateEditableInDatabase(newData);
    }
}
