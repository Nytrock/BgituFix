using System.Linq;
using UnityEngine;

public static class VectorAndString {
    public static string VectorToString(this Vector2 vector) {
        return $"{vector.x};{vector.y}";
    }

    public static Vector2 StringToVector(this string vector) {
        float[] values = vector.Split(';').Select(value => float.Parse(value)).ToArray();
        return new(values[0], values[1]);
    }
}
