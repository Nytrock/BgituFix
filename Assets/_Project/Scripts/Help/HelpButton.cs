using UnityEngine;
using UnityEngine.EventSystems;

public class HelpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private HelpPanel _helpPanel;
    [SerializeField] private float _secondsToWait;
    [SerializeField] private string _nameAndHotkeys;
    [TextArea][SerializeField] private string _description;

    private bool _isHolded;
    private bool _isShow;
    private float _timeHolded;

    public string Title => _nameAndHotkeys;
    public string Description => _description;

    private void Update() {
        if (!_isHolded || _isShow)
            return;

        _timeHolded += Time.deltaTime;
        CheckTimeHolded();
    }

    private void CheckTimeHolded() {
        if (_timeHolded < _secondsToWait)
            return;

        _helpPanel.ShowTip(this);
        _isShow = true;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _isHolded = true;
        _timeHolded = 0;
        _isShow = false;
    }

    public void OnPointerExit(PointerEventData eventData) {
        _isHolded = false;
        _isShow = false;
        _helpPanel.HideTip();
    }
}
