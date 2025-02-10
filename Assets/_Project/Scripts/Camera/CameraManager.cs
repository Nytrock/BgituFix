using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour {
    [SerializeField] private float _cameraOffset;
    [SerializeField] private float _minSize;
    [SerializeField] private float _scrollSensivity;
    [SerializeField] private float _keySpeed;

    private Camera _camera;
    private bool _isHover;
    private bool _isMoving = true;

    public static Vector3 LocalMousePosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void Awake() {
        _camera = GetComponent<Camera>();
    }

    private void Update() {
        if (_isHover)
            return;

        if (_isMoving)
            UpdatePosition();
        UpdateSize();
    }

    public void UpdateHover(bool isHover) {
        _isHover = isHover;
    }

    private void UpdateSize() {
        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
        if (scrollAxis == 0)
            return;

        _camera.orthographicSize = Mathf.Max(
            _camera.orthographicSize - scrollAxis * _scrollSensivity, _minSize
        );
    }

    private void UpdatePosition() {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        transform.position += _camera.orthographicSize * Time.deltaTime * _keySpeed * new Vector3(horizontalAxis, verticalAxis);
    }

    public void ForceSetSize(float size) {
        _camera.orthographicSize = size + size * _cameraOffset;
    }

    public void ResetPosition() {
        transform.position = new(0, 0, -10);
    }

    public void ChangeMovingState(bool isMoving) {
        _isMoving = isMoving;
    }
}
