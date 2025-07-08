using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComputerErrorTypeDropdown : CustomDropdown {
    [SerializeField] private Image _errorRenderer;

    public override void OnSelect(BaseEventData eventData) {
        base.OnSelect(eventData);
        UpdateErrorRenderer();
    }

    private void UpdateErrorRenderer() {
        ComputerErrorType type = (ComputerErrorType)(value + 1);
        _errorRenderer.color = ThemeManager.Instance.GetErrorColor(type);
    }
}
