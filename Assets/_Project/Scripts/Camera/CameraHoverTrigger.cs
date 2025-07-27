using UnityEngine;
using UnityEngine.EventSystems;

public class CameraHoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] protected CameraManager _cameraManager;

    public virtual void OnPointerEnter(PointerEventData eventData) {
        _cameraManager.UpdateHover(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData) {
        _cameraManager.UpdateHover(false);
    }

    public void OnDisable() {
        _cameraManager.UpdateHover(false);
    }

    public void SetManager(CameraManager cameraManager) {
        _cameraManager = cameraManager;
    }
}
