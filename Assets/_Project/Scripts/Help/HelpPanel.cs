using TMPro;
using UnityEngine;

public class HelpPanel : MonoBehaviour {
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;

    private void Awake() {
        ChangeState(false);
    }

    public void ShowTip(HelpButton helpButton) {
        _title.text = helpButton.Title;
        _description.text = helpButton.Description;
        transform.position = new(helpButton.transform.position.x, transform.position.y);
        ChangeState(true);
    }

    public void HideTip() {
        ChangeState(false);
    }

    private void ChangeState(bool newState) {
        _panel.SetActive(newState);
    }
}
