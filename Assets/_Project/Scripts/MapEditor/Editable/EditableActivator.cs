using UnityEngine;
using UnityEngine.EventSystems;

public class EditableActivator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField] private BaseEditable _editable;

    public void Update() {
        if (Input.GetMouseButtonUp(0) && _editable.IsResizing)
            _editable.ChangeEditState(false);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left)
            _editable.LeftButtonDown();
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left)
            _editable.LeftButtonUp();
        else if (eventData.button == PointerEventData.InputButton.Right)
            _editable.RightButtonUp();
    }
}
