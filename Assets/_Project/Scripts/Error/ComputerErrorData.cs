using System;
using UnityEngine;

[Serializable]
public class ComputerErrorData {
    [SerializeField] private int _id;
    [SerializeField] private int _computerId;
    [SerializeField] private int _userId;
    [SerializeField] private bool _isSolved;
    [SerializeField] private ComputerErrorType _type;
    [SerializeField] private DateTime _date;
    [SerializeField] private string _comment;
    private int _audienceId;

    public int Id => _id;
    public int ComputerId => _computerId;
    public int AudienceId => _audienceId;
    public int UserId => _userId;
    public bool IsSolved => _isSolved;
    public ComputerErrorType Type => _type;
    public DateTime Date => _date;
    public string Comment => _comment;

    public ComputerErrorData(int computerId, int userId, ComputerErrorType type, string comment) {
        _computerId = computerId;
        _userId = userId;
        _type = type;
        _comment = comment;

        _id = -1;
        _isSolved = false;
        _date = DateTime.Today;
    }

    public void SetAudienceId(int id) {
        _audienceId = id;
    }

    public void ChangeSolved(bool isSolved) {
        _isSolved = isSolved;
    }

    public void SetId(int id) {
        if (_id != -1)
            return;

        _id = id;
    }
}
