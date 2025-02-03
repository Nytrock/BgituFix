using UnityEngine;

public class EditableAudienceActivator : EditableActivator {
    [SerializeField] private Color _computerColor;
    [SerializeField] private Color _nonComputerColor;

    private AudienceData _data;

    public override void Setup(EditableData data) {
        base.Setup(data);

        _data = data as AudienceData;
        _renderer.color = _data.IsComputer ? _computerColor : _nonComputerColor;
    }
}
