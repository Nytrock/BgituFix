using System;
using UnityEngine;

[Serializable]
public class IdData {
    [SerializeField] protected int id;

    public int Id => id;
}
