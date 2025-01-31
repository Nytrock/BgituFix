using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData {
    [SerializeField] private BuildData[] _buildDatas;
    [SerializeField] private AudienceData[] _audienceDatas;
    [SerializeField] private ComputerData[] _computerDatas;

    public IEnumerable<BuildData> BuildDatas => _buildDatas;
    public IEnumerable<AudienceData> AudienceDatas => _audienceDatas;

    public MapData(BuildData[] buildDatas, AudienceData[] audienceDatas, ComputerData[] computerDatas) {
        _buildDatas = buildDatas;
        _audienceDatas = audienceDatas;
        _computerDatas = computerDatas;
    }

    public IEnumerable<AudienceData> GetAudiencesByBuild(BuildData buildData) {
        foreach (var data in _audienceDatas)
            if (data.BuildId == buildData.Id)
                yield return data;
    }

    public IEnumerable<ComputerData> GetComputersByAudience(AudienceData audienceData) {
        foreach (var data in _computerDatas)
            if (data.AudienceId == audienceData.Id)
                yield return data;
    }
}
