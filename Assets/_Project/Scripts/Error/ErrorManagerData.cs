using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ErrorManagerData {
    [SerializeField] private List<ComputerErrorData> _errorDatas;

    public IEnumerable<ComputerErrorData> Errors => _errorDatas;

    public ErrorManagerData(List<ComputerErrorData> errorDatas) {
        _errorDatas = errorDatas;
    }

    public void DeleteError(ComputerErrorData error) {
        _errorDatas.Remove(error);
    }
}
