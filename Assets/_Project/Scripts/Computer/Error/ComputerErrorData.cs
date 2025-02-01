using System;
using UnityEngine;

[Serializable]
public class ComputerErrorData {
    [SerializeField] private int _computerId;
    [SerializeField] private int _userId;
    [SerializeField] private bool _isSolved;
    [SerializeField] private ComputerErrorType _type;
    [SerializeField] private DateTime _date;
    [SerializeField] private string _comment;

    public int ComputerId => _computerId;
    public int UserId => _userId;
    public bool IsSolved => _isSolved;
    public ComputerErrorType Type => _type;
    public DateTime Date => _date;
    public string Comment => _comment;
}
