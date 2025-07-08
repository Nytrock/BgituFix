using System;
using UnityEngine;

[Serializable]
public class EditableComputerTypeIcon {
    [SerializeField] private Sprite _icon;
    [SerializeField] private ComputerType _type;

    public Sprite Icon => _icon;
    public ComputerType Type => _type;
}
