using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapBuildFloor : MonoBehaviour {
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private float _offset;

    private readonly List<EditableAudience> _audiences = new();

    public event Action<ComputerErrorType> ErrorUpdated;

    public float CameraSize {
        get {
            float verticalCenter = _renderer.transform.position.y;
            float verticalOffset = _renderer.size.y / 2f;
            float verticalMax = Mathf.Max(verticalCenter + verticalOffset, Mathf.Abs(verticalCenter - verticalOffset));

            float horizontalCenter = _renderer.transform.position.x;
            float horizontalOffset = _renderer.size.x / 2f;
            float horizontalMax = Mathf.Max(horizontalCenter + horizontalOffset, Mathf.Abs(horizontalCenter - horizontalOffset));

            return Mathf.Max(verticalMax, horizontalMax / 9f * 16);
        }
    }

    public void AddAudience(EditableAudience audience) {
        audience.transform.parent = transform;
        _audiences.Add(audience);
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }

    public void SetupSize() {
        Vector2 leftBottom, rigthTop;
        float minX = 0, minY = 0, maxX = 0, maxY = 0;

        if (_audiences.Count > 0) {
            leftBottom = _audiences[0].Position - _audiences[0].Size / 2f;
            rigthTop = _audiences[0].Position + _audiences[0].Size / 2f;
            minX = leftBottom.x;
            minY = leftBottom.y;
            maxX = rigthTop.x;
            maxY = rigthTop.y;
        }

        foreach (var audience in _audiences) {
            leftBottom = audience.Position - audience.Size / 2f;
            rigthTop = audience.Position + audience.Size / 2f;
            if (leftBottom.y < minY)
                minY = leftBottom.y;
            if (leftBottom.x < minX)
                minX = leftBottom.x;
            if (rigthTop.y > maxY)
                maxY = rigthTop.y;
            if (rigthTop.x > maxX)
                maxX = rigthTop.x;
        }

        leftBottom = new(minX - _offset, minY - _offset);
        rigthTop = new(maxX + _offset, maxY + _offset);
        _renderer.size = rigthTop - leftBottom;
        _renderer.transform.position = (rigthTop + leftBottom) / 2f;
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
