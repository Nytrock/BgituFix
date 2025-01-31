using TMPro;
using UnityEngine;

public class EditableAudience : Editable<AudienceData> {
    [SerializeField] private TextMeshProUGUI _nameText;
    private MapManager _mapManager;

    public override void Press() {
        _mapManager.OpenAudience(_data);
    }

    public override void Setup(AudienceData data) {
        base.Setup(data);
        _nameText.text = data.Name;
    }

    public void SetMapManager(MapManager mapManager) {
        _mapManager = mapManager;
    }
}
