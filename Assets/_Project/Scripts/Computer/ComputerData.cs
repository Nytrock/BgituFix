using System;
using UnityEngine;

[Serializable]
public class ComputerData : EditableData {
    [SerializeField] private string _serialNumber;
    [SerializeField] private int _audienceId;

    public string SerialNumber => _serialNumber;
    public int AudienceId => _audienceId;
}
