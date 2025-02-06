using System;
using UnityEngine;

[Serializable]
public class BuildData {
    [SerializeField] private int id;
    [SerializeField] private int number;
    [SerializeField] private int floors;

    public int Id => id;
    public int Number => number;
    public int FloorsCount => floors;
}
