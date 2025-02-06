using System;
using UnityEngine;

[Serializable]
public class IdData {
    [SerializeField] private int id;

    public int Id => id;
}
