using System;
using UnityEngine;

[Serializable]
public class IdData {
    [SerializeField] private int _id;

    public int Id => _id;
}
