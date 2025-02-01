using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MapData {
    [SerializeField] private BuildData[] _buildDatas;
    [SerializeField] private AudienceData[] _audienceDatas;
    [SerializeField] private ComputerData[] _computerDatas;
    [SerializeField] private ComputerErrorData[] _errorDatas;

    public IEnumerable<BuildData> BuildDatas => _buildDatas;
    public IEnumerable<AudienceData> AudienceDatas => _audienceDatas;

    public MapData(BuildData[] buildDatas, AudienceData[] audienceDatas,
        ComputerData[] computerDatas, ComputerErrorData[] errorDatas) {
        _buildDatas = buildDatas;
        _audienceDatas = audienceDatas;
        _computerDatas = computerDatas;
        _errorDatas = errorDatas.OrderBy(error => error.Type).ToArray();
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

    public IEnumerable<ComputerErrorData> GetErrorsByComputer(ComputerData computerData) {
        foreach (var data in _errorDatas)
            if (data.ComputerId == computerData.Id)
                yield return data;
    }

    public ComputerErrorType GetFloorMaxErrorType(int buildId, int floorIndex) {
        IEnumerable<int> audiences = _audienceDatas
            .Where(audience => audience.BuildId == buildId && audience.Floor == floorIndex)
            .Select(audience => audience.Id);
        IEnumerable<int> computers = _computerDatas
            .Where(computer => audiences.Contains(computer.AudienceId))
            .Select(computer => computer.Id);
        IEnumerable<ComputerErrorType> errors = _errorDatas
            .Where(error => computers.Contains(error.ComputerId))
            .Select(error => error.Type);

        if (errors.Count() == 0)
            return ComputerErrorType.None;
        return errors.Max();
    }

    public ComputerErrorType GetAudienceMaxErrorType(int audienceId) {
        IEnumerable<int> computers = _computerDatas
            .Where(computer => computer.AudienceId == audienceId)
            .Select(computer => computer.Id);
        IEnumerable<ComputerErrorType> errors = _errorDatas
            .Where(error => computers.Contains(error.ComputerId))
            .Select(error => error.Type);

        if (errors.Count() == 0)
            return ComputerErrorType.None;
        return errors.Max();
    }
}
