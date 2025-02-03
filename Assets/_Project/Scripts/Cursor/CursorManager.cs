using System;
using UnityEngine;

public class CursorManager : Singleton<CursorManager> {
    [SerializeField] private CursorTypeStyle[] cursorStyles;

    private void Start() {
        SetType(CursorType.Default);
    }

    public void SetType(CursorType type) {
        foreach (var style in cursorStyles)
            if (style.Type == type)
                SetStyle(style);
    }

    private void SetStyle(CursorTypeStyle style) {
        Cursor.SetCursor(style.Cursor, style.Offset, CursorMode.ForceSoftware);
    }
}

[Serializable]
public class CursorTypeStyle {
    [SerializeField] private Texture2D _cursor;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private CursorType _type;

    public Texture2D Cursor => _cursor;
    public Vector2 Offset => _offset;
    public CursorType Type => _type;
}