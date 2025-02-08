using UnityEngine;
using UnityEngine.EventSystems;

public class EditableActivator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField] private BaseEditable _editable;

    public void OnPointerDown(PointerEventData eventData) {

    }

    public void OnPointerUp(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left)
            _editable.LeftButtonUp();
    }
}
