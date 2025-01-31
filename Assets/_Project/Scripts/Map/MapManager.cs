using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class MapManager : MonoBehaviour {
    [SerializeField] private UserManager _userManager;
    [SerializeField] private MapBuildManager _buildManager;
    [SerializeField] private MapAudienceManager _audienceManager;
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
            GenerateStructures();
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
        GenerateStructures();
    }

    private void GenerateStructures() {
        _buildManager.GenerateBuilds(this);
        _audienceManager.GenerateAudiences(_data);
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
