using System;
using UnityEngine;

[Serializable]
public class BoolChangeData {
    [SerializeField] private int id;
    [SerializeField] private bool boolValue;

    public BoolChangeData(int id, bool boolValue) {
        this.id = id;
        this.boolValue = boolValue;
    }
}
