using TMPro;
using UnityEngine;

public class ComputerErrorRendererWithCount : ComputerErrorRenderer {
    [SerializeField] private TextMeshProUGUI _countText;
    private int _count = 0;

    public void SetCount(int count) {
        _count = count;
        UpdateText();
    }

    public void AddToCount() {
        _count++;
        UpdateText();
    }

    private void UpdateText() {
        _countText.text = _count.ToString();
    }
}
