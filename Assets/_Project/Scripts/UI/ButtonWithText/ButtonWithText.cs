using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithText : Button {
    [SerializeField] private TextMeshProUGUI _text;

    public void SetText(string text) {
        _text.text = text;
    }

    public void SetStyle(StateStyle style) {
        if (image != null)
            image.color = style.BackgroundColor;
        _text.color = style.TextColor;
    }
}
