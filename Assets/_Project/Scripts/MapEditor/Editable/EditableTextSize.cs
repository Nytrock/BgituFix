using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class EditableTextSize : MonoBehaviour {
    [SerializeField] private float _sizesTextsMultiplier;

    private TextMeshProUGUI _text;

    private void Awake() {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void SetSize(float size) {
        _text.text = size.ToString() + Units.SIZE_UNIT;
        _text.fontSize = size * _sizesTextsMultiplier;
    }
}
