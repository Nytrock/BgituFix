using TMPro;
using UnityEngine;

public class ComputerErrorRendererWithCount : ComputerErrorRenderer {
    [SerializeField] private TextMeshProUGUI _countText;
    private int _count = 0;

    public int Count => _count;

    public void AddToCount() {
        _count++;
        UpdateText();
        UpdateState();
    }

    public void RemoveFromCount() {
        if (_count == 0)
            return;

        _count--;
        UpdateText();
        UpdateState();
    }

    private void UpdateText() {
        _countText.text = _count.ToString();
    }

    public override void UpdateState() {
        gameObject.SetActive(_type != ComputerErrorType.None && _count > 0 && _isEdit);
    }
}
