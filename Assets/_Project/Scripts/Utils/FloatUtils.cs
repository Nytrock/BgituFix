using System;
using UnityEngine;

public static class FloatUtils {
    public static bool NearlyEqual(this float a, float b) {
        double absA = Math.Abs(a);
        double absB = Math.Abs(b);
        double diff = Math.Abs(a - b);

        if (a.Equals(b)) {
            return true;
        } else if (a == 0 || b == 0 || absA + absB < float.Epsilon) {
            return diff < float.Epsilon;
        } else {
            return diff / (absA + absB) < float.Epsilon;
        }
    }

    public static bool NearlyEqualOrLess(this float a, float b) {
        if (a < b)
            return true;
        return NearlyEqual(a, b);
    }

    public static bool NearlyEqualOrGreater(this float a, float b) {
        if (a > b)
            return true;
        return NearlyEqual(a, b);
    }

    public static float Round(this float num, int precision) {
        float coef = Mathf.Pow(10, precision);
        return Mathf.Round(num * coef) / coef;
    }
}
