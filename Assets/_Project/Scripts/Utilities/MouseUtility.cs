using UnityEngine;

public static class MouseUtility {
    public static Vector3 LocalMousePosition {
        get {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
