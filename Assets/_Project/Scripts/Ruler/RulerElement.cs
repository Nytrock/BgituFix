using UnityEngine;

public class RulerElement : MonoBehaviour {
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private RulerCanvas _canvas;

    private BaseMapElement _nowMapElement;

    private void Update() {
        if (!gameObject.activeSelf || _nowMapElement == null)
            return;

        Vector2 localMousePosition = CameraManager.LocalMousePosition;
        transform.position = localMousePosition;
        CalculateDistances();
    }

    private void CalculateDistances() {
        Vector2 rulerPosition = transform.position;
        _nowMapElement.GetBounds(out Vector2 leftDown, out Vector2 rightUp);
        bool isRulerInside = EditorUtils.RectContainsRect(leftDown, rightUp, rulerPosition, rulerPosition);

        _canvas.ChangeState(isRulerInside);
        if (!isRulerInside)
            return;

        float left = CalculateDistance(Vector2.left, leftDown.x).x;
        float right = CalculateDistance(Vector2.right, rightUp.x).x;
        float top = CalculateDistance(Vector2.up, rightUp.y).y;
        float bottom = CalculateDistance(Vector2.down, leftDown.y).y;

        if (left.NearlyEqual(right) || top.NearlyEqual(bottom)) {
            _canvas.ChangeState(false);
            return;
        }
        _canvas.SetArrows(left, right, top, bottom);
    }

    private Vector2 CalculateDistance(Vector2 direction, float defaultValue) {
        Vector2 rulerPosition = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(rulerPosition, direction);

        if (hit)
            return hit.point - rulerPosition;
        else
            return new Vector2(defaultValue, defaultValue) - rulerPosition;
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
        if (newState)
            _nowMapElement = _mapManager.GetNowMapElement();
    }
}
