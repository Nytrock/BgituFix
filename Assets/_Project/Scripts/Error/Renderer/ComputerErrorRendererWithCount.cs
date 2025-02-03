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
        ChangeState(true);
    }

    public void RemoveFromCount() {
        if (_count == 0)
            return;

        _count--;
        UpdateText();

        if (_count == 0)
            ChangeState(false);
    }

    private void UpdateText() {
        _countText.text = _count.ToString();
    }
}
