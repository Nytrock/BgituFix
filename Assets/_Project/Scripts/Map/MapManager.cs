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
    private MapState _state;

    public MapData Data => _data;
    public MapState State => _state;

    public event Action MapUpdated;
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
        _state = MapState.Build;
        MapLocationChanged?.Invoke();
    }

    public void OpenAudience(AudienceData audience) {
        _buildManager.ChangeState(false);
        _audienceManager.OpenAudience(audience);
        _state = MapState.Audience;
        MapLocationChanged?.Invoke();
    }

    public void RevertMapDataChanges(MapData oldMapData) {
        if (_state == MapState.Build)
            _buildManager.RevertEditingChanges(oldMapData);

        if (_state == MapState.Audience)
            _audienceManager.RevertEditingChanges(oldMapData);

        _data.CopyData(oldMapData);
        MapUpdated?.Invoke();
    }

    public IEnumerator SubmitMapDataChanges(MapData oldMapData) {
        _audienceManager.UpdateAudiences();
        yield return _buildManager.SubmitEditingChanges(oldMapData);
        yield return _audienceManager.SubmitEditingChanges(oldMapData);
        MapUpdated?.Invoke();
    }

    public void UpdateLocationSizeShow(bool isShow) {
        if (_state == MapState.Build)
            _buildManager.UpdateNowElementShowingSize(isShow);

        if (_state == MapState.Audience)
            _audienceManager.UpdateNowElementShowingSize(isShow);
    }

    public void CreateEmptyEditable() {
        if (_state == MapState.Build) {
            EditableAudience audience = _buildManager.CreateEmptyEditableOnMap();
            _data.AddAudience(audience.Data);
        }

        if (_state == MapState.Audience) {
            EditableComputer computer = _audienceManager.CreateEmptyEditableOnMap();
            _data.AddComputer(computer.Data);
        }
    }

    public IEnumerable<BaseEditable> PasteEditables(IEnumerable<EditableData> clipboard) {
        foreach (var data in clipboard) {
            if (_state == MapState.Build) {
                AudienceData audienceData = new(data as AudienceData);
                _data.CheckCopyAudienceData(audienceData, _buildManager.NowElement);
                EditableAudience audience = _buildManager.CreateEditableOnMap(audienceData, true);
                _data.AddAudience(audience.Data);
                yield return audience;
            }

            if (_state == MapState.Audience) {
                ComputerData computerData = new(data as ComputerData);
                _data.CheckCopyComputerData(computerData);
                EditableComputer computer = _audienceManager.CreateEditableOnMap(computerData, true);
                _data.AddComputer(computer.Data);
                yield return computer;
            }
        }
    }

    public void DeleteEditables(IEnumerable<BaseEditable> editables) {
        if (_state == MapState.Build) {
            foreach (var editable in editables)
                _buildManager.DeleteEditableOnMap(editable as EditableAudience);
        }

        if (_state == MapState.Audience) {
            foreach (var editable in editables)
                _audienceManager.DeleteEditableOnMap(editable as EditableComputer);
        }
    }

    public float GetPrecision() {
        if (_state == MapState.Build)
            return _buildManager.GridPrecision;
        return _audienceManager.GridPrecision;
    }

    public BaseMapElement GetNowMapElement() {
        if (_state == MapState.Build)
            return _buildManager.NowElement;
        return _audienceManager.NowElement;
    }
}
