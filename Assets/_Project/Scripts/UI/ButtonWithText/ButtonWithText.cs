using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithText : Button {
    [SerializeField] private TextMeshProUGUI _text;

    public void SetText(string text) {
        _text.text = text;
    }
}
