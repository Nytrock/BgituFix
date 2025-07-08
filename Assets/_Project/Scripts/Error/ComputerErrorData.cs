using System;
using UnityEngine;

[Serializable]
public class ComputerErrorData {
    [SerializeField] private int id;
    [SerializeField] private int computerId;
    [SerializeField] private int userId;
    [SerializeField] private bool isSolved;
    [SerializeField] private ComputerErrorType level;
    [SerializeField] private string createdAt;
    [SerializeField] private string description;
    private int _audienceId;

    public int Id => id;
    public int ComputerId => computerId;
    public int AudienceId => _audienceId;
    public int UserId => userId;
    public bool IsSolved => isSolved;
    public ComputerErrorType Type => level;
    public string Date => createdAt;
    public string Comment => description;

    public ComputerErrorData(int computerId, int userId, ComputerErrorType type, string comment) {
        id = -1;
        this.computerId = computerId;
        this.userId = userId;
        level = type;
        description = comment;
        isSolved = false;
    }

    public void SetId(int id) {
        if (this.id != -1)
            return;

        this.id = id;
    }

    public void SetAudienceId(int id) {
        _audienceId = id;
    }

    public void ChangeSolved(bool isSolved) {
        this.isSolved = isSolved;
    }

    public bool Equals(ComputerErrorData other) {
        return other.id == id && other.isSolved == isSolved;
    }
}
