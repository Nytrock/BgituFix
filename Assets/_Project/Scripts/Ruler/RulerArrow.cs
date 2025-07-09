using TMPro;
using UnityEngine;

public class RulerArrow : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _sizeText;
    [SerializeField] private RectTransform _minRect;
    [SerializeField] private RectTransform _maxRect;
    [SerializeField] private float _sizeMultiptier;

    public void SetSize(float min, float max) {
        float size = max - min;
        SetText(Mathf.Round(size * 100) / 100.0f);

        _minRect.sizeDelta = new(_minRect.sizeDelta.x, Mathf.Abs(min) * _sizeMultiptier);
        _maxRect.sizeDelta = new(_maxRect.sizeDelta.x, Mathf.Abs(max) * _sizeMultiptier);
    }

    private void SetText(float size) {
        _sizeText.text = size.ToString() + Units.SIZE_UNIT;
    }
}
