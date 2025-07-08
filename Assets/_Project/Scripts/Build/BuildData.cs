using System;
using UnityEngine;

[Serializable]
public class BuildData : IdData {
    [SerializeField] private int number;
    [SerializeField] private int floors;

    public int Number => number;
    public int FloorsCount => floors;
}
