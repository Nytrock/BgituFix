using System;
using UnityEngine;

[Serializable]
public class TokenData {
    [SerializeField] private string token;
    [SerializeField] private string refreshToken;

    public string Token => token;
    public string RefleshToken => refreshToken;
}
