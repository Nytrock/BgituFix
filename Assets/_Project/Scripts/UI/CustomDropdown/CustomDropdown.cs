using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomDropdown : TMP_Dropdown {
    [SerializeField] protected CameraManager _cameraManager;
    [SerializeField] protected Transform _canvas;

    public override void OnPointerClick(PointerEventData eventData) {
        base.OnPointerClick(eventData);
        MakeBlockerActuallyBlock();
    }

    private void MakeBlockerActuallyBlock() {
        Transform blocker = _canvas.GetChild(_canvas.childCount - 1);
        CameraHoverTrigger trigger = blocker.gameObject.AddComponent<CameraHoverTrigger>();
        trigger.SetManager(_cameraManager);
    }
}
