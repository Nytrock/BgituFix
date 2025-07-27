using System.Globalization;
using System.Linq;
using UnityEngine;

public static class VectorUtils {
    public static string ToSerializableString(this Vector2 vector) {
        NumberFormatInfo format = new CultureInfo("ru-RU").NumberFormat;
        string x = vector.x.ToString(format);
        string y = vector.y.ToString(format);
        return $"{x};{y}";
    }

    public static Vector2 ToVector(this string vector) {
        float[] values = vector.Split(';').Select(value => float.Parse(value, new CultureInfo("ru-RU").NumberFormat)).ToArray();
        return new(values[0], values[1]);
    }

    public static Vector3 Round(this Vector3 vector, int precision) {
        return new(vector.x.Round(precision), vector.y.Round(precision), vector.z.Round(precision));
    }
}