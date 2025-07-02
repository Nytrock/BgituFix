using System;
using UnityEngine;

[Serializable]
public class ErrorChangeData {
    [SerializeField] private int id;
    [SerializeField] private bool isSolved;

    public ErrorChangeData(int id, bool isSolved) {
        this.id = id;
        this.isSolved = isSolved;
    }
}
