using System;
using UnityEngine;

[Serializable]
public class BuildData {
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private int _floorsCount;

    public int Id => _id;
    public string Name => _name;
    public int FloorsCount => _floorsCount;
}
