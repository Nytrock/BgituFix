using UnityEngine;

public static class EditorUtils {
    public static void Resize(ref float length, ref float center, float mouseOffset, float mousePosition, float precision) {
        int sign = mouseOffset > 0 ? 1 : -1;
        float border = center + length / 2f * sign;
        if (border * sign < mousePosition * sign)
            return;

        float rawWidth = Mathf.Abs(border - mousePosition);
        length = SnapToGrid(rawWidth, precision);
        length = Mathf.Max(length, precision);
        center = border - length / 2f * sign;
    }

    public static Vector3 GetSnappedEditablePosition(Vector3 rawPosition, Vector3 size, float precision) {
        Vector3 leftBottom = rawPosition - size / 2f;
        leftBottom = SnapToGrid(leftBottom, precision);
        return leftBottom + size / 2f;
    }

    public static float SnapToGrid(float value, float precision) {
        return Mathf.Round(value / precision) * precision;
    }

    public static Vector2 SnapToGrid(Vector2 value, float precision) {
        return new(SnapToGrid(value.x, precision), SnapToGrid(value.y, precision));
    }

    public static bool RectContainsRect(Vector2 bigBottomLeft, Vector2 bigTopRight, Vector2 smallBottomLeft, Vector2 smallTopRight) {
        return bigBottomLeft.x.NearlyEqualOrLess(smallBottomLeft.x) &&
               bigBottomLeft.y.NearlyEqualOrLess(smallBottomLeft.y) &&
               bigTopRight.x.NearlyEqualOrGreater(smallTopRight.x) &&
               bigTopRight.y.NearlyEqualOrGreater(smallTopRight.y);
    }
}
