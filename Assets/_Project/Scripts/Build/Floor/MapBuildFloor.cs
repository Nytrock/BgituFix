using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapBuildFloor : MonoBehaviour {
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private float _offset;
    [SerializeField] private EditableTextSize _widthText;
    [SerializeField] private EditableTextSize _lengthText;

    private readonly List<EditableAudience> _audiences = new();
    private bool _isShowingSize;

    public event Action<ComputerErrorType> ErrorUpdated;

    public float CameraSize {
        get {
            float verticalCenter = _renderer.transform.position.y;
            float verticalOffset = _renderer.size.y / 2f;
            float verticalMax = Mathf.Max(verticalCenter + verticalOffset, Mathf.Abs(verticalCenter - verticalOffset));

            float horizontalCenter = _renderer.transform.position.x;
            float horizontalOffset = _renderer.size.x / 2f;
            float horizontalMax = Mathf.Max(horizontalCenter + horizontalOffset, Mathf.Abs(horizontalCenter - horizontalOffset));

            return Mathf.Max(verticalMax, horizontalMax / 16f * 9);
        }
    }

    private void Update() {
        if (!_isShowingSize)
            return;

        _widthText.SetSize(_renderer.size.x);
        _lengthText.SetSize(_renderer.size.y);
    }

    public void AddAudience(EditableAudience audience) {
        audience.transform.parent = transform;
        _audiences.Add(audience);
    }

    public void ChangeSizeShowState(bool newState) {
        _isShowingSize = newState;
        _canvas.gameObject.SetActive(newState);
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
        if (!newState)
            ChangeSizeShowState(false);
    }

    public void SetupSize() {
        float minX = 0, minY = 0, maxX = 0, maxY = 0;

        if (_audiences.Count > 0) {
            minX = _audiences[0].LeftBottom.x;
            minY = _audiences[0].LeftBottom.y;
            maxX = _audiences[0].RightTop.x;
            maxY = _audiences[0].RightTop.y;
        }

        foreach (var audience in _audiences) {
            if (audience.LeftBottom.y < minY)
                minY = audience.LeftBottom.y;
            if (audience.LeftBottom.x < minX)
                minX = audience.LeftBottom.x;
            if (audience.RightTop.y > maxY)
                maxY = audience.RightTop.y;
            if (audience.RightTop.x > maxX)
                maxX = audience.RightTop.x;
        }

        Vector2 leftBottom = new(minX - _offset, minY - _offset);
        Vector2 rigthTop = new(maxX + _offset, maxY + _offset);
        _renderer.size = rigthTop - leftBottom;
        _renderer.transform.position = (rigthTop + leftBottom) / 2f;
        _canvas.sizeDelta = _renderer.size * (1 / _canvas.localScale.x);
    }

    public void CheckNewError(ComputerErrorData data) {
        foreach (var audience in _audiences)
            audience.CheckNewError(data);
        UpdateError();
    }

    public void CheckDeletedError(ComputerErrorData data) {
        foreach (var audience in _audiences)
            audience.CheckDeletedError(data);
        UpdateError();
    }

    public void CheckChangedError(ComputerErrorData data) {
        foreach (var audience in _audiences)
            audience.CheckChangedError(data);
        UpdateError();
    }

    private void UpdateError() {
        if (_audiences.Count == 0) {
            ErrorUpdated?.Invoke(ComputerErrorType.None);
            return;
        }

        ComputerErrorType maxType = _audiences.Select(audience => audience.ErrorType).Max();
        ErrorUpdated?.Invoke(maxType);
    }

    public void RemoveAudience(EditableAudience audience) {
        _audiences.Remove(audience);
    }
}
