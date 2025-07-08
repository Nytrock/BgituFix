using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListData<T> {
    [SerializeField] private List<T> response;

    public IEnumerable<T> Response => response;
}
