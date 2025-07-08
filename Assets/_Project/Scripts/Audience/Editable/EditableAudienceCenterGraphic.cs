using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditableAudienceCenterGraphic : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _textMultiplier;
    [SerializeField] private Image _stairsImage;
    [SerializeField] private float _imageMultiplier;

    public void ChangeSize(Vector2 size) {
        float avgSize = (size.x + size.y) / 2f;
        _text.fontSize = avgSize * _textMultiplier;

        float stairsSize = avgSize * _imageMultiplier;
        _stairsImage.rectTransform.sizeDelta = new(stairsSize, stairsSize);
    }

    public void SetText(string text) {
        _text.text = text;
    }

    public void SetStyle(AudienceType type, Color color) {
        _text.color = color;
        _text.gameObject.SetActive(type != AudienceType.Stairs);

        _stairsImage.color = color;
        _stairsImage.gameObject.SetActive(type == AudienceType.Stairs);
    }
}
