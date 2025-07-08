using UnityEngine;

public class EditableCanvasInfo : MonoBehaviour {
    [SerializeField] protected MapEditManager _editManager;
    [SerializeField] private GameObject _panel;

    public void ChangeState(bool newState) {
        _panel.SetActive(newState);
    }
}
