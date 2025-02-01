using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour {
    [SerializeField] private float _minSize;
    [SerializeField] private float _scrollSensivity;
    private Camera _camera;

    private void Awake() {
        _camera = GetComponent<Camera>();
    }

    private void Update() {
        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
        if (scrollAxis == 0)
            return;

        _camera.orthographicSize = Mathf.Max(
            _camera.orthographicSize - scrollAxis * _scrollSensivity, _minSize
        );
    }

    public void ForceSetSize(float size) {
        _camera.orthographicSize = size;
    }

    public void SetSize(float size) {
        if (_camera.orthographicSize > size)
            return;

        _camera.orthographicSize = size;
    }
}
