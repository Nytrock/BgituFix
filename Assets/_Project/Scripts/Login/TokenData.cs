using System;
using UnityEngine;

[Serializable]
public class TokenData {
    [SerializeField] private string token;

    public string Token => token;
}
