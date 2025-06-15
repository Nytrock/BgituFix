using System.Globalization;
using System.Linq;
using UnityEngine;

public static class VectorAndString {
    public static string VectorToString(this Vector2 vector) {
        NumberFormatInfo format = new CultureInfo("ru-RU").NumberFormat;
        string x = vector.x.ToString(format);
        string y = vector.y.ToString(format);
        return $"{x};{y}";
    }

    public static Vector2 StringToVector(this string vector) {
        float[] values = vector.Split(';').Select(value => float.Parse(value, new CultureInfo("ru-RU").NumberFormat)).ToArray();
        return new(values[0], values[1]);
    }
}