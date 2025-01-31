using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class MapManager : MonoBehaviour {
    [SerializeField] private UserManager _userManager;
    [SerializeField] private MapBuildPool _buildPool;
    [SerializeField] private MapAudiencePool _audiencePool;
    [SerializeField] private string _APIPathToGetBuilds;
    [SerializeField] private string _APIPathToGetAudiences;
    [SerializeField] private string _APIPathToGetComputers;
    [SerializeField] private MapData _data;

    public event Action<bool> BlockStateChanged;

    private void Start() {
        CheckUserType();
    }

    private void CheckUserType() {
        UserType userType = _userManager.GetUserType();
        if (userType == UserType.None)
            BlockMap();
        else
            StartCoroutine(GenerateMap());
    }

    private void BlockMap() {
        gameObject.SetActive(false);
        BlockStateChanged?.Invoke(true);
    }

    private IEnumerator GenerateMap() {
        if (string.IsNullOrEmpty(_APIPathToGetBuilds)) {
            GenerateBuilds();
            GenerateAudiences();
            yield break;
        }

        UnityWebRequest www = RequestUtility.GetFromAPI(_APIPathToGetBuilds);
        yield return www.SendWebRequest();
        BuildData[] buildDatas = RequestUtility.ToData<BuildData[]>(www);

        www = RequestUtility.GetFromAPI(_APIPathToGetAudiences);
        yield return www.SendWebRequest();
        AudienceData[] audienceDatas = RequestUtility.ToData<AudienceData[]>(www);

        www = RequestUtility.GetFromAPI(_APIPathToGetComputers);
        yield return www.SendWebRequest();
        ComputerData[] computerDatas = RequestUtility.ToData<ComputerData[]>(www);

        _data = new(buildDatas, audienceDatas, computerDatas);
        GenerateBuilds();
        GenerateAudiences();
    }

    private void GenerateBuilds() {
        foreach (var buildData in _data.BuildDatas) {
            MapBuild build = _buildPool.GetObject();
            build.Setup(_data, buildData);
        }
    }

    private void GenerateAudiences() {
        foreach (var audienceData in _data.AudienceDatas) {
            MapAudience audience = _audiencePool.GetObject();
            audience.Setup(_data, audienceData);
        }
    }
}
