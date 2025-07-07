using TMPro;
using UnityEngine;

public class ComputerErrorRendererWithCount : ComputerErrorRenderer {
    [SerializeField] private TextMeshProUGUI _countText;
    private int _count = 0;

    public int Count => _count;

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
