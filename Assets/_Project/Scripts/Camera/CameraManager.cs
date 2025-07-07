using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour {
    [SerializeField] private float _cameraOffset;
    [SerializeField] private float _minSize;
    [SerializeField] private float _scrollSensivity;

    private Camera _camera;
    private float _startZoom;
    private bool _isHover;
    private bool _isEdit;
    private bool _canMove = true;
    private Vector3 _mouseOffset;

    public bool IsHover => _isHover;
    public float Zoom => _startZoom / _camera.orthographicSize;
    public Vector3 Size => new(_camera.orthographicSize * 2 / 9 * 16, _camera.orthographicSize * 2);
    public static Vector3 LocalMousePosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void Awake() {
        _camera = GetComponent<Camera>();
        _startZoom = _camera.orthographicSize;
    }

    private void Update() {
        if (_isHover)
            return;

        if (_canMove)
            UpdatePosition();
        UpdateSize();
    }

    public void UpdateHover(bool isHover) {
        _isHover = isHover;
    }

    public void ChangeEditState(bool isEdit) {
        _isEdit = isEdit;
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
        int buttonCode = _isEdit ? 1 : 0;
        if (Input.GetMouseButtonDown(buttonCode))
            _mouseOffset = LocalMousePosition;

        if (Input.GetMouseButton(buttonCode)) {
            Vector3 direction = _mouseOffset - LocalMousePosition;
            transform.position += direction;
        }
    }

    public void ForceSetSize(float size) {
        _camera.orthographicSize = size + size * _cameraOffset;
    }

    public void ResetPosition() {
        transform.position = new(0, 0, -10);
    }

    public void ChangeMoveState(bool isMoving) {
        _canMove = isMoving;
    }
}
