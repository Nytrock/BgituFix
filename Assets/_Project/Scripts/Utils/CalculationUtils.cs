using UnityEngine;

public static class CalculationUtils {
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

    public static void Resize(ref float length, ref float center, float mouseOffset, float mousePosition) {
        int sign = mouseOffset > 0 ? 1 : -1;
        float border = center + length / 2f * sign;
        if (border * sign < mousePosition * sign)
            return;

        length = Mathf.Abs(border - mousePosition);
        center = border - length / 2f * sign;
    }

    public static Vector3 GetSnappedPosition(Vector3 rawPosition, float precision) {
        float x = SnapToGrid(rawPosition.x, precision);
        float y = SnapToGrid(rawPosition.y, precision);
        return new(x, y);
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
