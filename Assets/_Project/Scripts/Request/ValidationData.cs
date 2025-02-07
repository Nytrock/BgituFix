using System;
using UnityEngine;

[Serializable]
public class ValidationData {
    [SerializeField] private bool isValid;

    public bool IsValid => isValid;
}
