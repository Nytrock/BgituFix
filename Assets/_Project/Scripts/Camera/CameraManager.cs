using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour {
    [SerializeField] private float _cameraOffset;
    [SerializeField] private float _minSize;
    [SerializeField] private float _zoomSensivity;

    private Camera _camera;
    private Vector3 _touchOffset;
    private bool _isHover;

    public static Vector3 LocalMousePosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void Awake() {
        _camera = GetComponent<Camera>();
    }

    private void Update() {
        if (_isHover)
            return;

        if (Input.touchCount == 1)
            UpdatePosition();
        else if (Input.touchCount == 2)
            UpdateSize();
    }

    public void UpdateHover(bool isHover) {
        _isHover = isHover;
    }

    private void UpdateSize() {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrev = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrev = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrev - touchOnePrev).magnitude;
        float nowMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = nowMagnitude - prevMagnitude;
        _camera.orthographicSize = Mathf.Max(_camera.orthographicSize - difference * _zoomSensivity, _minSize);
    }

    private void UpdatePosition() {
        if (Input.GetMouseButtonDown(0))
            _touchOffset = LocalMousePosition;

        if (Input.GetMouseButton(0)) {
            Vector3 direction = _touchOffset - LocalMousePosition;
            transform.position += direction;
        }
    }

    public void ForceSetSize(float size) {
        _camera.orthographicSize = size + size * _cameraOffset;
    }

    public void ResetPosition() {
        transform.position = new(0, 0, -10);
    }
}
