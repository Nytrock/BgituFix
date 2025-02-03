using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ErrorManagerData {
    [SerializeField] private List<ComputerErrorData> _errorDatas;

    public IEnumerable<ComputerErrorData> Errors => _errorDatas;

    public ErrorManagerData(List<ComputerErrorData> errorDatas) {
        _errorDatas = errorDatas;
    }

    internal void AddError(ComputerErrorData newError) {
        _errorDatas.Add(newError);
        int maxId = _errorDatas.Select(error => error.Id).Max();
        newError.SetId(maxId + 1);
    }

    public void DeleteError(ComputerErrorData error) {
        _errorDatas.Remove(error);
    }
}
