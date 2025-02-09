using UnityEngine;
using UnityEngine.EventSystems;

public class EditableActivator : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private BaseEditable _editable;

    public void OnPointerClick(PointerEventData eventData) {
        _editable.Click();
    }
}
