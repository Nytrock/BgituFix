using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MapManager : MonoBehaviour {
    [SerializeField] private UrlManager _urlManager;
    [SerializeField] private UserManager _userManager;
    [SerializeField] private MapBuildManager _buildManager;
    [SerializeField] private MapAudienceManager _audienceManager;
    [SerializeField] private ErrorManager _errorManager;
    [SerializeField] private string _APIPathToGetBuilds;
    [SerializeField] private string _APIPathToGetAudiences;
    [SerializeField] private string _APIPathToGetComputers;

    [SerializeField] private MapData _data;

    public MapData Data => _data;

    public event Action<bool> BlockStateChanged;

    private void Start() {
        CheckUserType();
    }

    private void CheckUserType() {
        if (_userManager.ClientType == UserType.None)
            BlockMap();
        else
            StartCoroutine(GetMap());
    }

    private void BlockMap() {
        gameObject.SetActive(false);
        BlockStateChanged?.Invoke(true);
    }

    private IEnumerator GetMap() {
        if (string.IsNullOrEmpty(_APIPathToGetBuilds)) {
            GenerateMap();
            yield break;
        }

        string token = _urlManager.GetParameter("token");

        UnityWebRequest request = RequestUtility.APIGet(_APIPathToGetBuilds, token);
        yield return request.SendWebRequest();
        List<BuildData> buildDatas = RequestUtility.ToData<List<BuildData>>(request);

        request = RequestUtility.APIGet(_APIPathToGetAudiences, token);
        yield return request.SendWebRequest();
        List<AudienceData> audienceDatas = RequestUtility.ToData<List<AudienceData>>(request);

        request = RequestUtility.APIGet(_APIPathToGetComputers, token);
        yield return request.SendWebRequest();
        List<ComputerData> computerDatas = RequestUtility.ToData<List<ComputerData>>(request);

        _data = new(buildDatas, audienceDatas, computerDatas);
        GenerateMap();
    }

    private void GenerateMap() {
        _buildManager.GenerateBuilds(this);
        _audienceManager.GenerateAudiences(this);
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

    public IEnumerator RevertMapData(MapData oldMapData) {
        yield break;
    }

    public IEnumerator CheckUpdatedData(MapData oldMapData) {
        yield break;
    }
}
