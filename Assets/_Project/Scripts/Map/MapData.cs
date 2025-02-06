using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MapData {
    [SerializeField] private List<BuildData> buildings;
    [SerializeField] private List<AudienceData> auditoriums;
    [SerializeField] private List<ComputerData> computers;

    public IEnumerable<BuildData> BuildDatas => buildings;
    public IEnumerable<AudienceData> AudienceDatas => auditoriums;
    public IEnumerable<ComputerData> ComputerDatas => computers;

    public MapData(IEnumerable<BuildData> buildDatas, IEnumerable<AudienceData> audienceDatas, IEnumerable<ComputerData> computerDatas) {
        buildings = buildDatas.OrderBy(build => build.Number).ToList();
        auditoriums = audienceDatas.ToList();
        computers = computerDatas.ToList();

        foreach (var data in auditoriums)
            data.SetupVectors();
        foreach (var data in computers)
            data.SetupVectors();
    }

    public IEnumerable<AudienceData> GetAudiencesByBuild(BuildData buildData) {
        foreach (var data in auditoriums)
            if (data.BuildId == buildData.Id)
                yield return data;
    }

    public IEnumerable<ComputerData> GetComputersByAudience(AudienceData audienceData) {
        foreach (var data in computers)
            if (data.AudienceId == audienceData.Id)
                yield return data;
    }

    public ComputerData GetComputerById(int id) {
        foreach (var data in computers)
            if (data.Id == id)
                return data;
        return null;
    }

    public AudienceData GetAudienceById(int id) {
        foreach (var data in auditoriums)
            if (data.Id == id)
                return data;
        return null;
    }

    public void AddAudience(AudienceData audienceData) {
        auditoriums.Add(audienceData);
    }

    public void DeleteAudience(AudienceData audienceData) {
        List<ComputerData> computersToDelete = new();
        foreach (var computerData in computers)
            if (computerData.AudienceId == audienceData.Id)
                computersToDelete.Add(computerData);

        foreach (var data in computersToDelete)
            DeleteComputer(data);

        auditoriums.Remove(audienceData);
    }

    public void AddComputer(ComputerData computerData) {
        computers.Add(computerData);
    }

    public void DeleteComputer(ComputerData computerData) {
        computers.Remove(computerData);
    }

    public bool ContainsAudience(AudienceData data) {
        return auditoriums.Select(audience => audience.Id).Contains(data.Id);
    }

    public bool ContainsComputer(ComputerData data) {
        return computers.Select(computer => computer.Id).Contains(data.Id);
    }
}
