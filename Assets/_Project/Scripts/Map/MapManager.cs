using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MapManager : MonoBehaviour {
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
    public event Action MapUpdated;
    public event Action MapGenerated;

    private void Awake() {
        _userManager.ClientSetuped += StartGettingMap;
    }

    private void StartGettingMap() {
        StartCoroutine(GetMapData());
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
        StateChanged?.Invoke(newState);
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
        _buildManager.GenerateBuilds(_data);
        _audienceManager.GenerateAudiences(_data);
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

    public EditableAudience CreateEmptyAudience() {
        int buildId = _buildManager.NowElement.Id;
        int floorIndex = _buildManager.NowElement.NowFloor;
        int audiencesCount = _data.AudienceDatas.Count();
        return CreateAudience(new(buildId, floorIndex, audiencesCount));
    }

    public EditableAudience CreateAudience(AudienceData audienceData) {
        _data.AddAudience(audienceData);
        return _buildManager.CreateEditableOnMap(audienceData);
    }

    public void DeleteAudience(EditableAudience audience) {
        _buildManager.DeleteEditableOnMap(audience);
        _data.DeleteAudience(audience.Data);
    }

    public EditableComputer CreateEmptyComputer() {
        int audienceId = _audienceManager.NowElement.Id;
        int computersCount = _data.ComputerDatas.Count();
        return CreateComputer(new(audienceId, computersCount));
    }

    public EditableComputer CreateComputer(ComputerData computerData) {
        _data.AddComputer(computerData);
        return _audienceManager.CreateEditableOnMap(computerData);
    }

    public void DeleteComputer(EditableComputer computer) {
        _audienceManager.DeleteEditableOnMap(computer);
        _data.DeleteComputer(computer.Data);
    }

    public void RevertMapDataChanges(MapData oldMapData) {
        if (_buildManager.NowElement != null)
            RevertBuildDataChanges(oldMapData);

        if (_audienceManager.NowElement != null)
            RevertAudienceDataChanges(oldMapData);

        _data = oldMapData;
        MapUpdated?.Invoke();
    }

    private void RevertBuildDataChanges(MapData oldMapData) {
        foreach (var oldData in oldMapData.AudienceDatas) {
            if (!_data.ContainsAudience(oldData)) {
                _buildManager.CreateEditableOnMap(oldData);
            } else {
                _buildManager.ChangeEditableOnMap(oldData);
                _audienceManager.UpdateAudienceById(oldData);
            }
        }

        foreach (var newData in _data.AudienceDatas)
            if (!oldMapData.ContainsAudience(newData))
                _buildManager.DeleteEditableOnMap(newData);
    }

    private void RevertAudienceDataChanges(MapData oldMapData) {
        foreach (var oldData in oldMapData.ComputerDatas) {
            if (!_data.ContainsComputer(oldData))
                _audienceManager.CreateEditableOnMap(oldData);
            else
                _audienceManager.ChangeEditableOnMap(oldData);
        }

        foreach (var newData in _data.ComputerDatas)
            if (!oldMapData.ContainsComputer(newData))
                _audienceManager.DeleteEditableOnMap(newData);
    }

    public IEnumerator SubmitMapDataChanges(MapData oldMapData) {
        _audienceManager.UpdateAudiences();
        yield return SubmitBuildDataChanges(oldMapData);
        yield return SubmitAudienceDataChanges(oldMapData);
        MapUpdated?.Invoke();
    }

    private IEnumerator SubmitBuildDataChanges(MapData oldMapData) {
        foreach (var oldData in oldMapData.AudienceDatas) {
            if (!_data.ContainsAudience(oldData)) {
                foreach (var computerData in _data.GetComputersByAudience(oldData))
                    _errorManager.DeleteErrorsByComputerId(computerData);
                _audienceManager.DeleteAudience(oldData);
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
    }

    private IEnumerator SubmitAudienceDataChanges(MapData oldMapData) {
        foreach (var oldData in oldMapData.ComputerDatas) {
            if (!_data.ContainsComputer(oldData)) {
                _errorManager.DeleteErrorsByComputerId(oldData);
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
