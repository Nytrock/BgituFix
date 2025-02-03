using UnityEngine;

public static class RectUtility {
    public static bool ContainsLocalMouse(this RectTransform rect) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,
            Input.mousePosition, Camera.main, out Vector2 localMousePosition);
        return rect.rect.Contains(localMousePosition);
    }

    public static bool ContainsMouse(this RectTransform rect) {
        Vector2 localMousePosition = rect.InverseTransformPoint(Input.mousePosition);
        return rect.rect.Contains(localMousePosition);
    }
}
