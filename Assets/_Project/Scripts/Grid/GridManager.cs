using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    [SerializeField] private LineRenderer _verticalLine;
    [SerializeField] private LineRenderer _horizontalLine;
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private float _defaultWidth;

    private bool _isActive;
    private float _precision;

    public bool IsActive => _isActive;

    public event Action StateChanged;

    public void ChangeState(bool newState) {
        ChangeStateSilently(newState);
        StateChanged?.Invoke();
    }

    public void ChangeStateSilently(bool newState) {
        _isActive = newState;
        _horizontalLine.gameObject.SetActive(newState);
        _verticalLine.gameObject.SetActive(newState);
    }

    private void Update() {
        if (!_isActive)
            return;

        DrawLines();
    }

    private void DrawLines() {
        Vector3 offset = new(_precision, _precision);
        Vector2 bottomLeft = _cameraManager.transform.position - _cameraManager.Size / 2f - offset;
        Vector2 topRight = _cameraManager.transform.position + _cameraManager.Size / 2f + offset;

        bottomLeft = EditorUtils.SnapToGrid(bottomLeft, _precision);
        topRight = EditorUtils.SnapToGrid(topRight, _precision);

        bool direction = false;
        List<Vector3> horizontalLinePoints = new();
        List<Vector3> verticalLinePoints = new();

        float x = bottomLeft.x, y = bottomLeft.y;
        while (x < topRight.x) {
            if (direction) {
                verticalLinePoints.Add(new(x, topRight.y));
                verticalLinePoints.Add(new(x, bottomLeft.y));
            } else {
                verticalLinePoints.Add(new(x, bottomLeft.y));
                verticalLinePoints.Add(new(x, topRight.y));
            }

            x += _precision;
            direction = !direction;
        }

        direction = false;
        while (y < topRight.y) {
            if (direction) {
                horizontalLinePoints.Add(new(topRight.x, y));
                horizontalLinePoints.Add(new(bottomLeft.x, y));
            } else {
                horizontalLinePoints.Add(new(bottomLeft.x, y));
                horizontalLinePoints.Add(new(topRight.x, y));
            }

            y += _precision;
            direction = !direction;
        }

        SetupLine(_horizontalLine, horizontalLinePoints);
        SetupLine(_verticalLine, verticalLinePoints);
    }

    private void SetupLine(LineRenderer line, List<Vector3> points) {
        line.positionCount = points.Count;
        line.widthCurve = AnimationCurve.Constant(0, 1, _defaultWidth * 1 / _cameraManager.Zoom);
        line.SetPositions(points.ToArray());
    }

    public void SetPrecision(float precision) {
        _precision = precision;
    }
}
