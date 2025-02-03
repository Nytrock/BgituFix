using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData {
    [SerializeField] private List<BuildData> _buildDatas;
    [SerializeField] private List<AudienceData> _audienceDatas;
    [SerializeField] private List<ComputerData> _computerDatas;

    public IEnumerable<BuildData> BuildDatas => _buildDatas;
    public IEnumerable<AudienceData> AudienceDatas => _audienceDatas;

    public MapData(List<BuildData> buildDatas, List<AudienceData> audienceDatas, List<ComputerData> computerDatas) {
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

    public ComputerData GetComputerById(int id) {
        foreach (var data in _computerDatas)
            if (data.Id == id)
                return data;
        return null;
    }

    public AudienceData GetAudienceById(int id) {
        foreach (var data in _audienceDatas)
            if (data.Id == id)
                return data;
        return null;
    }
}
