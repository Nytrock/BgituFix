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
        SetupVectors();
    }

    public void SetupVectors() {
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

    public bool ContainsEditable(EditableData data) {
        if (data is AudienceData audienceData)
            return ContainsAudience(audienceData);
        if (data is ComputerData computerData)
            return ContainsComputer(computerData);
        throw new NotImplementedException();
    }

    public void DeleteEditable(EditableData data) {
        if (data is AudienceData audienceData)
            DeleteAudience(audienceData);
        if (data is ComputerData computerData)
            DeleteComputer(computerData);
    }

    public void CopyData(MapData oldMapData) {
        auditoriums = oldMapData.auditoriums;
        buildings = oldMapData.buildings;
        computers = oldMapData.computers;
    }

    public void CheckCopyAudienceData(AudienceData copyAudience, MapBuild nowBuild) {
        int counter = 0;
        string originalName = copyAudience.Name.RemoveCopyName();

        foreach (var audience in auditoriums)
            if (audience.Name.StartsWith(originalName))
                counter++;

        copyAudience.ChangeBuild(nowBuild.Id, nowBuild.NowFloor);
        copyAudience.UpdateName($"{originalName} ({counter})");
    }

    public void CheckCopyComputerData(ComputerData copyComputer) {
        int counter = 0;
        string originalName = copyComputer.SerialNumber.RemoveCopyName();

        foreach (var computer in computers)
            if (computer.SerialNumber.StartsWith(originalName))
                counter++;

        copyComputer.UpdateNumber($"{originalName} ({counter})");
    }

    public bool Equals(MapData other) {
        foreach (var audience in auditoriums) {
            AudienceData otherAudience = other.GetAudienceById(audience.Id);
            if (otherAudience == null) return false;
            if (!otherAudience.Equals(audience)) return false;
        }

        foreach (var audience in other.auditoriums) {
            AudienceData myAudience = GetAudienceById(audience.Id);
            if (myAudience == null) return false;
            if (!myAudience.Equals(audience)) return false;
        }

        foreach (var computer in computers) {
            ComputerData otherComputer = other.GetComputerById(computer.Id);
            if (otherComputer == null) return false;
            if (!otherComputer.Equals(computer)) return false;
        }

        foreach (var computer in other.computers) {
            ComputerData myComputer = GetComputerById(computer.Id);
            if (myComputer == null) return false;
            if (!myComputer.Equals(computer)) return false;
        }

        return true;
    }
}
