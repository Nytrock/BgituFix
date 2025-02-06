using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ErrorManagerData {
    [SerializeField] private List<ComputerErrorData> breakdowns;

    public IEnumerable<ComputerErrorData> Errors => breakdowns;

    public void AddError(ComputerErrorData newError) {
        if (breakdowns.Contains(newError)) return;

        breakdowns.Add(newError);
    }

    public void DeleteError(ComputerErrorData error) {
        breakdowns.Remove(error);
    }

    public bool Contains(ComputerErrorData error) {
        return breakdowns.Select(error => error.Id).Contains(error.Id);
    }

    public ComputerErrorData GetErrorById(int id) {
        foreach (var error in breakdowns)
            if (error.Id == id)
                return error;
        return null;
    }
}
