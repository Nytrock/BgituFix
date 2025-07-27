using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditableCanvasBorderElement : CameraHoverTrigger, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
    [SerializeField] private CursorType _cursorType;

    public bool IsHover { get; private set; }

    public CursorType CursorType => _cursorType;

    public event Action<CursorType> CursorTypeChanged;
    public event Action LeftButtonDown;
    public event Action LeftButtonUp;

    private void Start() {
        _cameraManager = CameraManager.Instance;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        LeftButtonDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData) {
        LeftButtonUp?.Invoke();
    }

    public override void OnPointerEnter(PointerEventData eventData) {
        base.OnPointerEnter(eventData);
        IsHover = true;
        CursorTypeChanged?.Invoke(_cursorType);
    }

    public override void OnPointerExit(PointerEventData eventData) {
        base.OnPointerExit(eventData);
        IsHover = false;
        CursorTypeChanged?.Invoke(CursorType.Default);
    }
}
