using UnityEngine;
using UnityEngine.EventSystems;

public class CameraHoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private CameraManager _cameraManager;

    public void SetCameraManager(CameraManager cameraManager) {
        _cameraManager = cameraManager;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _cameraManager.UpdateHover(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        _cameraManager.UpdateHover(false);
    }
}
