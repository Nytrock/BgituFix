using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditableCanvasBorderElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    [SerializeField] private CursorType _cursorType;

    public event Action<CursorType> CursorTypeChanged;
    public event Action LeftButtonClick;

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        LeftButtonClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        CursorTypeChanged?.Invoke(_cursorType);
    }

    public void OnPointerExit(PointerEventData eventData) {
        CursorTypeChanged?.Invoke(CursorType.Default);
    }
}
